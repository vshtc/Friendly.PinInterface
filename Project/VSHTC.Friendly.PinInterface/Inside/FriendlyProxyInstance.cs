using System;
using System.Reflection;
using Codeer.Friendly;

namespace VSHTC.Friendly.PinInterface.Inside
{
    class FriendlyProxyInstance<TInterface> : FriendlyProxy<TInterface>, IAppVarOwner
        where TInterface : IInstance
    {
        public AppVar AppVar { get; private set; }

        public FriendlyProxyInstance(AppVar appVar)
            : base(appVar.App) 
        {
            AppVar = appVar;
        }

        protected override AppVar Invoke(MethodInfo method, string name, object[] args, Async async, OperationTypeInfo typeInfo)
        {
            return FriendlyInvokeSpec.GetFriendlyOperation(AppVar, name, async, typeInfo)(args);
        }
    }
}
