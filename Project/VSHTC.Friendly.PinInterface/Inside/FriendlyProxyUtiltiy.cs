using Codeer.Friendly;
using Codeer.Friendly.Dynamic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace VSHTC.Friendly.PinInterface.Inside
{
    internal static class FriendlyProxyUtiltiy
    {

        internal static bool HasInterface(Type ownerType, Type inType)
        {
            return ownerType.GetInterfaces().Any(e => e == inType);
        }

        internal static T WrapFriendlyProxy<T>(Type proxyType, params object[] args)
        {
            var friendlyProxyType = proxyType.MakeGenericType(typeof(T));
            dynamic friendlyProxy = Activator.CreateInstance(friendlyProxyType, args);
            return (T)friendlyProxy.GetTransparentProxy();
        }


        internal static bool TryExecuteSpecialInterface<TInterface>(MethodInfo method, object[] args, ref Async asyncNext, ref OperationTypeInfo operationTypeInfoNext, out object retunObject)
        {
            retunObject = null;

            //GetTypeだけは相性が悪い
            //思わぬところで呼び出され、シリアライズできず、クラッシュしてしまう。
            if (InterfacesSpec.IsGetType(method))
            {
                retunObject = typeof(TInterface);
                return true;
            }

            if (InterfacesSpec.IsAsyncNext(method))
            {
                if (asyncNext != null)
                {
                    throw new NotSupportedException("既に次回呼び出しのAsyncは設定されています。");
                }
                asyncNext = new Async();
                retunObject = asyncNext;
                return true;
            }
            if (InterfacesSpec.IsOperationTypeInfoNext(method))
            {
                if (operationTypeInfoNext != null)
                {
                    throw new NotSupportedException("既に次回呼び出しのOperationTypeInfoは設定されています。");
                }
                operationTypeInfoNext = (OperationTypeInfo)args[0];
                return true;
            }
            return false;
        }


        internal static string GetInvokeName(MethodInfo method)
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

        internal static string GetCommas(int count)
        {
            StringBuilder b = new StringBuilder();
            for (int i = 0; i < count; i++)
            {
                b.Append(",");
            }
            return b.ToString();
        }

        internal static void AdjustRefOutArgs(AppFriend app, MethodInfo method, bool isAsyunc, object[] src, out object[] args, out  Func<object>[] refoutArgsFunc)
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
                    AdjustRefOutArgs(app, parameters[i].ParameterType.GetElementType(), src[i], out arg, out refoutFunc);
                    args[i] = arg;
                    refoutArgsFunc[i] = isAsyunc ? (() => GetDefault(parameters[i].ParameterType)) : refoutFunc;
                }
                else
                {
                    object srcObj = src[i];
                    refoutArgsFunc[i] = () => srcObj;
                    args[i] = srcObj;
                }
            }
        }
        internal static void AdjustRefOutArgs(AppFriend app, Type type, object src, out object arg, out Func<object> refoutFunc)
        {
            if (type.IsInterface)
            {
                if (src == null)
                {
                    AppVar appVar = app.Dim();
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
                        AppVar appVar = app.Dim();
                        arg = appVar;
                        refoutFunc = () => WrapChildAppVar(type, appVar);
                    }
                }
            }
            else
            {
                AppVar appVar = app.Copy(src);
                arg = appVar;
                refoutFunc = () => appVar.Core;
            }
        }

        internal static object ToReturnObject(bool isAsync, AppVar returnedAppVal, ParameterInfo parameterInfo)
        {
            if (parameterInfo.ParameterType == typeof(void))
            {
                return null;
            }
            else if (parameterInfo.ParameterType.IsInterface)
            {
                return WrapChildAppVar(parameterInfo.ParameterType, returnedAppVal);
            }
            else if (parameterInfo.ParameterType == typeof(AppVar))
            {
                return returnedAppVal;//@@@特殊処理。どこかに判定入れて、IAppVarOwnerの場合以外は例外
            }
            else
            {
                return isAsync ? GetDefault(parameterInfo.ParameterType) : returnedAppVal.Core;
            }
        }


        interface IValue
        {
            object Value { get; }
        }
        class DefaultValue<T> : IValue
        {
            public object Value { get { return default(T); } }
        }
        internal static object GetDefault(Type type)
        {
            IValue value = Activator.CreateInstance(typeof(DefaultValue<>).MakeGenericType(type), new object[] { }) as IValue;
            return value.Value;
        }

        internal static object WrapChildAppVar(Type type, AppVar ret)
        {
            var friendlyProxyType = typeof(FriendlyProxyInstance<>).MakeGenericType(type);
            dynamic friendlyProxy = Activator.CreateInstance(friendlyProxyType, new object[] { ret });
            return friendlyProxy.GetTransparentProxy();
        }


        internal static FriendlyOperation GetFriendlyOperation(dynamic target, string name, Async async, OperationTypeInfo typeInfo)
        {
            if (async != null && typeInfo != null)
            {
                return target[name, typeInfo, async];
            }
            else if (async != null)
            {
                return target[name, async];
            }
            else if (typeInfo != null)
            {
                return target[name, typeInfo];
            }
            return target[name];
        }


        internal static object[] ToRefOutArgs(Func<object>[] refoutArgsFunc)
        {
            var refoutArgs = refoutArgsFunc.Select(e => e()).ToArray();
            return refoutArgs;
        }
    }
}
