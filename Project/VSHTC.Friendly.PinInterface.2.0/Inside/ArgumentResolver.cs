using System;
using System.Reflection;
using Codeer.Friendly;
using System.Collections.Generic;
using System.Runtime.Remoting;

namespace VSHTC.Friendly.PinInterface.Inside
{
    class ArgumentResolver
    {
        delegate object ResolveArgument();
        internal object[] InvokeArguments { get; private set; }
        ResolveArgument[] _resolveArguments;

        internal ArgumentResolver(AppFriend app, MethodInfo method, bool isAsyunc, object[] src)
        {
            InvokeArguments = new object[src.Length];
            _resolveArguments = new ResolveArgument[src.Length];
            var parameters = method.GetParameters();
            for (int i = 0; i < parameters.Length; i++)
            {
                Type paramType = parameters[i].ParameterType;
                object srcObj = src[i];
                if (paramType.IsByRef)
                {
                    Type elementType = paramType.GetElementType();
                    object arg;
                    ResolveArgument refoutFunc;
                    ResolveRefOutArgs(app, elementType, srcObj, out arg, out refoutFunc);
                    InvokeArguments[i] = arg;
                    _resolveArguments[i] = (isAsyunc && elementType != typeof(AppVar)) ? (() => TypeUtility.GetDefault(elementType)) : refoutFunc;
                }
                else
                {
                    InvokeArguments[i] = TryConvertProxy(srcObj);
                    _resolveArguments[i] = () => srcObj;
                }
            }
        }

        internal object[] GetRefOutArgs()
        {
            object[] args = new object[_resolveArguments.Length];
            for (int i = 0; i < _resolveArguments.Length; i++)
            {
                args[i] = _resolveArguments[i]();
            }
            return args;
        }

        static void ResolveRefOutArgs(AppFriend app, Type type, object src, out object arg, out ResolveArgument resolveArgument)
        {
            if (type.IsInterface)
            {
                ResolveInterfaceRefOutArgs(app, type, src, out arg, out resolveArgument);
            }
            else if (type == typeof(AppVar))
            {
                ResolveAppVarRefOutArgs(app, type, src, out arg, out resolveArgument);
            }
            else if (UserWrapperUtility.IsAppVarWrapper(type))
            {
                ResolveUserWrapperRefOutArgs(app, type, src, out arg, out resolveArgument);
            }
            else
            {
                AppVar appVar = app.Dim(src);
                arg = appVar;
                resolveArgument = () => appVar.Core;
            }
        }

        static void ResolveInterfaceRefOutArgs(AppFriend app, Type type, object src, out object arg, out ResolveArgument resolveArgument)
        {
            if (src == null)
            {
                AppVar appVar = app.Dim();
                arg = appVar;
                resolveArgument = () => FriendlyProxyFactory.WrapFriendlyProxyInstance(type, appVar);
            }
            else
            {
                var proxy = RemotingServices.GetRealProxy(src) as IAppVarOwner;
                if (proxy == null)
                {
                    AppVar appVar = app.Dim(src);
                    arg = appVar;
                    resolveArgument = () => FriendlyProxyFactory.WrapFriendlyProxyInstance(type, appVar);
                }
                else
                {
                    arg = proxy;
                    resolveArgument = () => AppVarUtility.IsNull(proxy.AppVar) ? null : src;
                }
            }
        }
        
        static void ResolveAppVarRefOutArgs(AppFriend app, Type type, object src, out object arg, out ResolveArgument resolveArgument)
        {
            if (src == null)
            {
                AppVar appVar = app.Dim();
                arg = appVar;
                resolveArgument = () => appVar;
            }
            else
            {
                arg = src;
                resolveArgument = () => src;
            }
        }

        static void ResolveUserWrapperRefOutArgs(AppFriend app, Type type, object src, out object arg, out ResolveArgument resolveArgument)
        {
            if (src == null)
            {
                AppVar appVar = app.Dim();
                arg = appVar;
                resolveArgument = () => UserWrapperUtility.CreateWrapper(type, appVar);
            }
            else
            {
                arg = src;
                IAppVarOwner owner = src as IAppVarOwner;
                resolveArgument = () => (owner != null && AppVarUtility.IsNull(owner.AppVar)) ? null : src;
            }
        }

        static object TryConvertProxy(object obj)
        {
            var proxy = RemotingServices.GetRealProxy(obj) as IAppVarOwner;
            return proxy == null ? obj : proxy;
        }
    }
}
