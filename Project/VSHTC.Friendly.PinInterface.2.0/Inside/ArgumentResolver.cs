using System;
using System.Reflection;
using Codeer.Friendly;
using System.Collections.Generic;

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
                    bool isInterface = TypeUtility.HasInterface(elementType, typeof(IInstance));
                    _resolveArguments[i] = (isAsyunc && !isInterface) ? (() => TypeUtility.GetDefault(elementType)) : refoutFunc;
                }
                else
                {
                    InvokeArguments[i] = srcObj;
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
            if (TypeUtility.HasInterface(type, typeof(IInstance)))
            {
                ResolveAppVarRefOutArgs(app, type, src, FriendlyProxyFactory.WrapFriendlyProxyInstance, out arg, out resolveArgument);
            }
            else if (type == typeof(AppVar))
            {
                ResolveAppVarRefOutArgs(app, type, src, (t, appVar) => appVar, out arg, out resolveArgument);
            }
            else if (UserWrapperUtility.IsAppVarWrapper(type))
            {

                ResolveAppVarRefOutArgs(app, type, src, UserWrapperUtility.CreateWrapper, out arg, out resolveArgument);
            }
            else
            {
                AppVar appVar = app.Dim(src);
                arg = appVar;
                resolveArgument = () => appVar.Core;
            }
        }

        delegate object ResolveAppVar(Type type, AppVar appVar);
        private static void ResolveAppVarRefOutArgs(AppFriend app, Type type, object src, ResolveAppVar resolver, out object arg, out ResolveArgument resolveArgument)
        {
            if (src == null)
            {
                AppVar appVar = app.Dim();
                arg = appVar;
                resolveArgument = () => resolver(type, appVar);
            }
            else
            {
                arg = src;
                resolveArgument = () => src;
            }
        }
    }
}
