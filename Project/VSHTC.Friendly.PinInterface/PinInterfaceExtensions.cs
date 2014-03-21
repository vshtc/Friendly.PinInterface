using Codeer.Friendly;
using System;
using System.Linq;
using VSHTC.Friendly.PinInterface.Inside;

namespace VSHTC.Friendly.PinInterface
{
    public static class PinInterfaceExtensions
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
    }
}
