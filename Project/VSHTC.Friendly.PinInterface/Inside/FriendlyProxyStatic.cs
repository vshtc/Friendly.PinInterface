using System;
using System.Reflection;
using Codeer.Friendly;

namespace VSHTC.Friendly.PinInterface.Inside
{
    class FriendlyProxyStatic<TInterface> : FriendlyProxy<TInterface>
        where TInterface : IStatic
    {
        string _typeFullName;

        public FriendlyProxyStatic(AppFriend app, string typeFullName)
            : base(app)
        {
            _typeFullName = typeFullName;
        }

        protected override AppVar Invoke(MethodInfo method, string name, object[] args, ref Async async, ref OperationTypeInfo typeInfo)
        {
            try
            {
                return FriendlyInvokeSpec.GetFriendlyOperation(App, _typeFullName + "." + name, async, typeInfo)(args);
            }
            finally
            {
                async = null;
                typeInfo = null;
            }
        }
    }
}

