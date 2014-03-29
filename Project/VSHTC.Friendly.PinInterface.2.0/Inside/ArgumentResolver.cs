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
                    object arg;
                    ResolveArgument refoutFunc;
                    ResolveRefOutArgs(app, paramType.GetElementType(), srcObj, out arg, out refoutFunc);
                    InvokeArguments[i] = arg;
                    _resolveArguments[i] = isAsyunc ? (() => TypeUtility.GetDefault(paramType)) : refoutFunc;
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

        internal static OperationTypeInfo TryCreateOperationTypeInfo(AppFriend app, string declaringType, MethodInfo method)
        {
            if (string.IsNullOrEmpty(declaringType))
            {
                return null;
            }
            List<string> arguments = new List<string>();
            foreach (var arg in method.GetParameters())
            {
                string argTarget = TargetTypeUtility.GetFullName(app, arg.ParameterType);
                if (string.IsNullOrEmpty(argTarget))
                {
                    return null;
                }
                arguments.Add(argTarget);
            }
            return new OperationTypeInfo(declaringType, arguments.ToArray());
        }

        static void ResolveRefOutArgs(AppFriend app, Type type, object src, out object arg, out ResolveArgument resolveArgument)
        {
            if (type.IsInterface)
            {
                if (src == null)
                {
                    AppVar appVar = app.Dim();
                    arg = appVar;
                    resolveArgument = () => FriendlyProxyFactory.WrapFriendlyProxyInstance(type, appVar);
                }
                else
                {
                    IAppVarOwner appVarOwner = src as IAppVarOwner;
                    if (appVarOwner != null)
                    {
                        arg = src;
                        resolveArgument = () => src;
                    }
                    else
                    {
                        AppVar appVar = app.Dim();
                        arg = appVar;
                        resolveArgument = () => FriendlyProxyFactory.WrapFriendlyProxyInstance(type, appVar);
                    }
                }
            }
            else
            {
                AppVar appVar = app.Dim(src);
                arg = appVar;
                resolveArgument = () => appVar.Core;
            }
        }
    }
}
