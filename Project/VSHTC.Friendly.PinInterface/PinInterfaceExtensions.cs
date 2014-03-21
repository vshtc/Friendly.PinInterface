using Codeer.Friendly;
using System;

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
    }
}
