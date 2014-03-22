using Codeer.Friendly;
using System;
using System.Reflection;

namespace VSHTC.Friendly.PinInterface.Inside
{
    class FriendlyProxyInstance<TInterface> : FriendlyProxy<TInterface>
    {
        private readonly AppVar _appVar;

        public AppVar AppVar {
            get { return _appVar; }
        }

        public FriendlyProxyInstance(AppFriend app, AppVar appVar)
            : base(app)
        {
            _appVar = appVar;
        }

        protected override AppVar Invoke(MethodInfo method, object[] args)
        {

            if ((method.DeclaringType == typeof(IAppVarOwner) && method.Name == "get_AppVar") ||
                (method.DeclaringType == typeof(IAppVarSelf) && method.Name == "get_CodeerFriendlyAppVar"))
            {
                return _appVar;
            }

            return _appVar[method.Name](args);
        }
    }
}
