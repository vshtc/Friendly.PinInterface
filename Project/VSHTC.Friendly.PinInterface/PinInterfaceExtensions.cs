using Codeer.Friendly;
using System;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Proxies;
using VSHTC.Friendly.PinInterface.Inside;

namespace VSHTC.Friendly.PinInterface
{
    public static class PinInterfaceExtensions
    {
        public static TInterface Pin<TInterface>(this AppVar appVar, AppFriend app)
        {
            return PinHelper.Instance<TInterface>(app, appVar);
        }

        public static TInterface Pin<TInterface>(this AppFriend app)
        {
            return PinHelper.Static<TInterface>(app);
        }

        public static TInterface Pin<TInterface, TTarget>(this AppFriend app)
        {
            return PinHelper.Static<TInterface, TTarget>(app);
        }

        public static TInterface Static<TInterface>(this AppFriend app, Type targetType)
        {
            return PinHelper.Static<TInterface>(app, targetType);
        }

        public static TInterface Static<TInterface>(this AppFriend app, string targetTypeFullName)
        {
            return PinHelper.Static<TInterface>(app, targetTypeFullName);
        }


        public static TInterface Cast<TInterface>(this IFriendlyProxy source) {
            if (source == null) {
                throw new ArgumentNullException("source");
            }
            //TODO:もう少し細かい場合わけが必要
            RealProxy proxy = System.Runtime.Remoting.RemotingServices.GetRealProxy(source);
            if (proxy == null) {
                return (TInterface)source;
            }
            var sourceType = proxy.GetType();
            if (sourceType.IsGenericType) {
                if (sourceType.GetGenericTypeDefinition() == typeof(FriendlyProxyInstance<>)) {
                    dynamic d = proxy;
                    return (TInterface)new FriendlyProxyInstance<TInterface>((AppFriend)d.App, (AppVar)d.AppVar).GetTransparentProxy();
                }
            }
            
            return (TInterface)source;
        }
    }
}
