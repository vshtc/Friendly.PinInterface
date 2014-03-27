using System;
using System.Linq;
using System.Runtime.Remoting.Proxies;
using VSHTC.Friendly.PinInterface.Inside;
using Codeer.Friendly;
using VSHTC.Friendly.PinInterface.BaseInterfaces;

namespace VSHTC.Friendly.PinInterface
{
    public static class InterfaceHelper
    {
        public static TInterface Pin<TInterface>(this AppVar appVar)
             where TInterface : IInstance
        {
            return (TInterface)new FriendlyProxyInstance<TInterface>(appVar).GetTransparentProxy();
        }

        public static TInterface Pin<TInterface, TTarget>(this AppFriend app)
        where TInterface : IAppFriendFunctions
        {
            return Pin<TInterface>(app, typeof(TTarget));
        }

        public static TInterface Pin<TInterface>(this AppFriend app, Type targetType)
        where TInterface : IAppFriendFunctions
        {
            return Pin<TInterface>(app, targetType.FullName);
        }

        public static TInterface Pin<TInterface>(this AppFriend app, string targetTypeFullName)
        where TInterface : IAppFriendFunctions
        {
             Type proxyType = typeof(TInterface).GetInterfaces().Any(e => e == typeof(IStatic)) ?
                typeof(FriendlyProxyStatic<>) : typeof(FriendlyProxyConstructor<>);
            return WrapProxy<TInterface>(proxyType, app, targetTypeFullName);
        }

        public static TInterface Cast<TInterface>(this IAppVarOwner source)
        where TInterface : IInstance
        {
            return Pin<TInterface>(source.AppVar);
        }

        private static T WrapProxy<T>(Type proxyType, params object[] args)
        {
            var friendlyProxyType = proxyType.MakeGenericType(typeof(T));
            dynamic friendlyProxy = Activator.CreateInstance(friendlyProxyType, args);
            return (T)friendlyProxy.GetTransparentProxy();
        }
    }
}
