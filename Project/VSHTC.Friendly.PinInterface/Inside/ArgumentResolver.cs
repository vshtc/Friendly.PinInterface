using System;
using System.Linq;
using System.Reflection;
using Codeer.Friendly;
using Codeer.Friendly.Dynamic;

namespace VSHTC.Friendly.PinInterface.Inside
{
    class ArgumentResolver
    {
        internal object[] InvokeArguments { get; private set; }
        Func<object>[] _refoutArgsFunc;

        internal ArgumentResolver(AppFriend app, MethodInfo method, bool isAsyunc, object[] src)
        {
            InvokeArguments = new object[src.Length];
            _refoutArgsFunc = new Func<object>[src.Length];
            var parameters = method.GetParameters();
            for (int i = 0; i < parameters.Length; i++)
            {
                Type paramType = parameters[i].ParameterType;
                object srcObj = src[i];
                if (paramType.IsByRef)
                {
                    object arg;
                    Func<object> refoutFunc;
                    ResolveRefOutArgs(app, paramType.GetElementType(), srcObj, out arg, out refoutFunc);
                    InvokeArguments[i] = arg;
                    _refoutArgsFunc[i] = isAsyunc ? (() => TypeUtility.GetDefault(paramType)) : refoutFunc;
                }
                else
                {
                    InvokeArguments[i] = srcObj;
                    _refoutArgsFunc[i] = () => srcObj;
                }
            }
        }

        internal object[] GetRefOutArgs()
        {
            return _refoutArgsFunc.Select(e => e()).ToArray();
        }

        static void ResolveRefOutArgs(AppFriend app, Type type, object src, out object arg, out Func<object> refoutFunc)
        {
            if (type.IsInterface)
            {
                if (src == null)
                {
                    AppVar appVar = app.Dim();
                    arg = appVar;
                    refoutFunc = () => FriendlyProxyFactory.WrapFriendlyProxyInstance(type, appVar);
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
                        refoutFunc = () => FriendlyProxyFactory.WrapFriendlyProxyInstance(type, appVar);
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
    }
}
