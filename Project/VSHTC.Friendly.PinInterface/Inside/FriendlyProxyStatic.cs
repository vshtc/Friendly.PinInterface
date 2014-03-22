using Codeer.Friendly;
using System;
using System.Reflection;

namespace VSHTC.Friendly.PinInterface.Inside
{
    class FriendlyProxyStatic<TInterface> : FriendlyProxy<TInterface>
    {
        string _typeFullName;

        public FriendlyProxyStatic(AppFriend app, string typeFullName)
            : base(app)
        {
            _typeFullName = typeFullName;
        }

        protected override AppVar Invoke(Type declaringType, string name, object[] args)
        {
            return App[_typeFullName + "." + name](args);
        }
    }
}
