using System;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Remoting.Proxies;
using System.Runtime.CompilerServices;
using Codeer.Friendly;
using Codeer.Friendly.Dynamic;
using System.Text;

namespace VSHTC.Friendly.PinInterface.Inside
{
    abstract class FriendlyProxy<TInterface> : RealProxy
    {
        public AppFriend App { get; private set; }

        public FriendlyProxy(AppFriend app)
            : base(typeof(TInterface)) 
        {
            App = app;
        }

        public override IMessage Invoke(IMessage msg)
        {
            var mm = msg as IMethodMessage;
            var method = (MethodInfo)mm.MethodBase;
            string invokeName = GetInvokeName(method);

            //GetTypeだけは相性が悪い
            //思わぬところで呼び出され、シリアライズできず、クラッシュしてしまう。
            if ((method.DeclaringType == typeof(object) && invokeName == "GetType"))
            {
                return new ReturnMessage(typeof(TInterface), null, 0, mm.LogicalCallContext, (IMethodCallMessage)msg);
            }

            CheckDynamicArguments(method.GetParameters());

            //out ref対応
            object[] args;
            Func<object>[] refoutArgsFunc;
            AdjustRefOutArgs(method, mm.Args, out args, out refoutArgsFunc);

            //呼び出し
            var returnedAppVal = Invoke(method.DeclaringType, invokeName, args);

            //戻り値とout,refの処理
            object objReturn = ToReturnObject(returnedAppVal, method.ReturnParameter);
            var refoutArgs = refoutArgsFunc.Select(e => e()).ToArray();
            return new ReturnMessage(objReturn, refoutArgs, refoutArgs.Length, mm.LogicalCallContext, (IMethodCallMessage)msg);
        }

        private string GetInvokeName(MethodInfo method)
        {
            //配列とその他の[]アクセスの差分を吸収する処理
            string invokeName = method.Name;
            if (invokeName == "get_Item")
            {
                return "[" + GetCommas(method.GetParameters().Length - 1) + "]";
            }
            else if (invokeName == "set_Item")
            {
                return "[" + GetCommas(method.GetParameters().Length - 2) + "]";
            }
            //プロパティーが指し示すものがフィールドである場合の対応
            else if (invokeName.IndexOf("get_") == 0 ||
                    invokeName.IndexOf("set_") == 0)
            {
                return invokeName.Substring(4);
            }
            return invokeName;
        }

        private string GetCommas(int count)
        {
            StringBuilder b = new StringBuilder();
            for (int i = 0; i < count; i++)
            {
                b.Append(",");
            }
            return b.ToString();
        }

        protected abstract AppVar Invoke(Type declaringType, string name, object[] args);

        private void CheckDynamicArguments(ParameterInfo[] parameterInfo)
        {
            foreach (var element in parameterInfo)
            {
                if (element.GetCustomAttributes(false).Any(e=>e.GetType() == typeof(DynamicAttribute)))
                {
                    throw new NotSupportedException("さすがに引数dynamicはやめて");
                }
            }
        }

        private void AdjustRefOutArgs(MethodInfo method, object[] src, out object[] args, out  Func<object>[] refoutArgsFunc)
        {
            args = new object[src.Length];
            refoutArgsFunc = new Func<object>[src.Length];
            var parameters = method.GetParameters();
            for (int i = 0; i < parameters.Length; i++)
            {
                if (parameters[i].ParameterType.IsByRef)
                {
                    object arg;
                    Func<object> refoutFunc;
                    AdjustRefOutArgs(parameters[i].ParameterType.GetElementType(), src[i], out arg, out refoutFunc);
                    args[i] = arg;
                    refoutArgsFunc[i] = refoutFunc;
                }
                else
                {
                    args[i] = src[i];
                    object srcObj = src[i];
                    refoutArgsFunc[i] = () => srcObj;
                }
            }
        }

        private void AdjustRefOutArgs(Type type, object src, out object arg, out Func<object> refoutFunc)
        {
            if (type.IsInterface)
            {
                if (src == null)
                {
                    AppVar appVar = App.Dim();
                    arg = appVar;
                    refoutFunc = () => WrapChildAppVar(type, appVar);
                }
                else
                {
                    IAppVarOwner appVarOwner = src as IAppVarOwner;
                    if (appVarOwner != null)
                    {
                        arg = src;
                        refoutFunc = () => src;
                    }
                    else
                    {
                        AppVar appVar = App.Dim();
                        arg = appVar;
                        refoutFunc = () => WrapChildAppVar(type, appVar);
                    }
                }
            }
            else if (type == typeof(AppVar))
            {
                if (src == null)
                {
                    AppVar appVar = App.Dim();
                    arg = appVar;
                    refoutFunc = () => appVar;
                }
                else
                {
                    arg = src;
                    refoutFunc = () => src;
                }
            }
            else
            {
                AppVar appVar = App.Copy(src);
                arg = appVar;
                refoutFunc = () => appVar.Core;
            }
        }

        private static object ToReturnObject(AppVar returnedAppVal, ParameterInfo parameterInfo)
        {
            if (parameterInfo.ParameterType == typeof(void))
            {
                return null;
            }
            else if (parameterInfo.ParameterType == typeof(AppVar))
            {
                return returnedAppVal;
            }
            else if (parameterInfo.ParameterType.IsInterface)
            {
                return WrapChildAppVar(parameterInfo.ParameterType, returnedAppVal);
            }
            else if (parameterInfo.GetCustomAttributes(false).Any(o => o is DynamicAttribute))
            {
                return returnedAppVal.Dynamic();
            }
            else
            {
                return returnedAppVal.Core;
            }
        }

        private static object WrapChildAppVar(Type type, AppVar ret)
        {
            var friendlyProxyType = typeof (FriendlyProxyInstance<>).MakeGenericType(type);
            dynamic friendlyProxy = Activator.CreateInstance(friendlyProxyType, new object[] {ret});
            return friendlyProxy.GetTransparentProxy();
        }

        
    }
}