using System;
using System.Reflection;
using Codeer.Friendly;

namespace VSHTC.Friendly.PinInterface.Inside
{
    class FriendlyProxyInstance<TInterface> : FriendlyProxy<TInterface>
        where TInterface : IInstance
    {
        private readonly AppVar _appVar;

        public FriendlyProxyInstance(AppVar appVar)
            : base(appVar.App) 
        {
            _appVar = appVar;
        }

        protected override AppVar Invoke(MethodInfo method, string name, object[] args, ref Async async, ref OperationTypeInfo typeInfo)
        {
            if (InterfacesSpec.IsAppVar(method))
            {
                return _appVar;
            }
            try
            {
                return FriendlyProxyUtiltiy.GetFriendlyOperation(_appVar, name, async, typeInfo)(args);
            }
            finally
            {
                async = null;
                typeInfo = null;
            }
        }
    }
}
