using System;
using System.Reflection;
using Codeer.Friendly;
using System.Collections.Generic;

namespace VSHTC.Friendly.PinInterface.Inside
{
    class FriendlyProxyInstance<TInterface> : FriendlyProxy<TInterface>, IAppVarOwner, IModifyAsync
    {
        bool _isComObject;

        public AppVar AppVar { get; private set; }

        protected override bool IsNeedAutoOperationTypeInfo { get { return _isComObject; } }
     
        public FriendlyProxyInstance(AppVar appVar)
            : base(appVar.App) 
        {
            AppVar = appVar;
            _isComObject = !AppVar.IsNull && (string)AppVar["GetType"]()["FullName"]().Core == "System.__ComObject";
        }

        protected override AppVar Invoke(MethodInfo method, string name, object[] args, Async async, OperationTypeInfo typeInfo)
        {
            return FriendlyInvokeSpec.GetFriendlyOperation(AppVar, name, async, typeInfo)(args);
        }

        protected override string GetTargetTypeFullName(MethodInfo method)
        {
            return _isComObject ? method.DeclaringType.FullName : TargetTypeUtility.GetFullName(App, typeof(TInterface));
        }

        public Async AsyncNext()
        {
            Async async = new Async();
            SetAsyncNext(async);
            return async;
        }
    }
}
