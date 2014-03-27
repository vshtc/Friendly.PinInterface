using Codeer.Friendly;
using System;
using System.Linq;
using System.Runtime.Remoting.Proxies;
using VSHTC.Friendly.PinInterface.Inside;

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
        where TInterface : IStatic
        {
            return Pin<TInterface>(app, typeof(TTarget));
        }

        public static TInterface Pin<TInterface>(this AppFriend app, Type targetType)
        where TInterface : IStatic
        {
            return Pin<TInterface>(app, targetType.FullName);
        }

        public static TInterface Pin<TInterface>(this AppFriend app, string targetTypeFullName)
        where TInterface : IStatic
        {
            return (TInterface)new FriendlyProxyStatic<TInterface>(app, targetTypeFullName).GetTransparentProxy();
        }

        public static TInterface Cast<TInterface>(this IAppVarOwner source)
             where TInterface : IInstance
        {
            return Pin<TInterface>(source.AppVar);
        }
    }
}
