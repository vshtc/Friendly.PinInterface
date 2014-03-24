using Codeer.Friendly;
using System;
using System.Reflection;

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

        public AppVar AppVar {
            get { return _appVar; }
        }

        protected override AppVar Invoke(MethodInfo method, string name, object[] args, ref Async async, ref OperationTypeInfo typeInfo)
        {
            if ((method.DeclaringType == typeof(IAppVarOwner) && name == "AppVar"))
            {
                return _appVar;
            }
            try
            {
                return GetFriendlyOperation(_appVar, name, async, typeInfo)(args);
            }
            finally
            {
                async = null;
                typeInfo = null;
            }
        }
    }
}
