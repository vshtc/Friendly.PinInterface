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
        {
            return (TInterface)new FriendlyProxyInstance<TInterface>(appVar).GetTransparentProxy();
        }

        public static TInterface Pin<TInterface>(this AppFriend app)
        {
            var attributes = typeof(TInterface).GetCustomAttributes(false).Where(o => o is ProxyTragetAttribute).Select(o => (ProxyTragetAttribute)o).ToArray();
            if (attributes.Length != 1)
            {
                throw new NotSupportedException("staticの場合は対象のタイプを明示してください");
            }
            return Pin<TInterface>(app, attributes[0].TargetTypeFullName);
        }

        public static TInterface Pin<TInterface, TTarget>(this AppFriend app)
        {
            return Pin<TInterface>(app, typeof(TTarget));
        }

        public static TInterface Pin<TInterface>(this AppFriend app, Type targetType)
        {
            return Pin<TInterface>(app, targetType.FullName);
        }

        public static TInterface Pin<TInterface>(this AppFriend app, string targetTypeFullName)
        {
            return (TInterface)new FriendlyProxyStatic<TInterface>(app, targetTypeFullName).GetTransparentProxy();
        }



        public static TInterface Cast<TInterface>(this IFriendlyProxy source)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            //TODO:もう少し細かい場合わけが必要
            RealProxy proxy = System.Runtime.Remoting.RemotingServices.GetRealProxy(source);
            if (proxy == null)
            {
                return (TInterface)source;
            }
            var sourceType = proxy.GetType();
            if (sourceType.IsGenericType)
            {
                if (sourceType.GetGenericTypeDefinition() == typeof(FriendlyProxyInstance<>))
                {
                    dynamic d = proxy;
                    return (TInterface)new FriendlyProxyInstance<TInterface>((AppVar)d.AppVar).GetTransparentProxy();
                }
            }

            return (TInterface)source;
        }
    }
}
