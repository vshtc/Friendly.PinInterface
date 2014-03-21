using Codeer.Friendly;
using System.Reflection;

namespace VSHTC.Friendly.PinInterface.Inside
{
    class FriendlyProxyInstance<TInterface> : FriendlyProxy<TInterface>
    {
        private readonly AppVar _appVar;

        public FriendlyProxyInstance(AppVar appVar)
            : base(appVar.App)
        {
            _appVar = appVar;
        }

        protected override AppVar Invoke(MethodInfo method, object[] args)
        {

            if ((method.DeclaringType == typeof(IAppVarOwner) && method.Name == "get_AppVar"))
            {
                return _appVar;
            }

            return _appVar[method.Name](args);
        }
    }
}
