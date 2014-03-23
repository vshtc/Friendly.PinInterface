using Codeer.Friendly;
using System;
using System.Linq;
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

        protected override AppVar Invoke(MethodInfo method, string name, object[] args)
        {
            if (method.GetCustomAttributes(true).Any(e => e.GetType() == typeof(NewAttribute)))
            {
                return App.Dim(new NewInfo(_typeFullName, args));
            }
            return App[_typeFullName + "." + name](args);
        }
    }
}
