using System;
using System.Reflection;
using Codeer.Friendly;

namespace VSHTC.Friendly.PinInterface.Inside
{
    class FriendlyProxyConstructor<TInterface> : FriendlyProxy<TInterface>
    where TInterface : IConstructor
    {
        string _typeFullName;

        public FriendlyProxyConstructor(AppFriend app, string typeFullName)
            : base(app)
        {
            _typeFullName = typeFullName;
        }

        protected override AppVar Invoke(MethodInfo method, string name, object[] args, Async async, OperationTypeInfo typeInfo)
        {
            return (typeInfo == null) ?
                     App.Dim(new NewInfo(_typeFullName, args)) :
                     App.Dim(new NewInfo(_typeFullName, args), typeInfo);
        }
    }
}
