using Codeer.Friendly;
using System;
using System.Linq;
using VSHTC.Friendly.PinInterface.Inside;

namespace VSHTC.Friendly.PinInterface
{
    public static class PinHelper
    {
        public static TInterface Instance<TInterface>(AppFriend app, AppVar appVar)
        {
            return (TInterface)new FriendlyProxyInstance<TInterface>(app, appVar).GetTransparentProxy();
        }

        public static TInterface Static<TInterface>(AppFriend app)
        {
            var attributes = typeof(TInterface).GetCustomAttributes(false).Where(o => o is ProxyTragetAttribute).Select(o => (ProxyTragetAttribute)o).ToArray();
            if (attributes.Length != 1)
            {
                throw new NotSupportedException("staticの場合は対象のタイプを明示してください");
            }
            return Static<TInterface>(app, attributes[0].TargetTypeFullName);
        }

        public static TInterface Static<TInterface, TTarget>(AppFriend app)
        {
            return Static<TInterface>(app, typeof(TTarget));
        }

        public static TInterface Static<TInterface>(AppFriend app, Type targetType)
        {
            return Static<TInterface>(app, targetType.FullName);
        }

        public static TInterface Static<TInterface>(AppFriend app, string targetTypeFullName)
        {
            return (TInterface)new FriendlyProxyStatic<TInterface>(app, targetTypeFullName).GetTransparentProxy();
        }
    }
}
