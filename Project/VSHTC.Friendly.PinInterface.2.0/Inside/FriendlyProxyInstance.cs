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

        public FriendlyProxyInstance(AppVar appVar)
            : base(appVar.App) 
        {
            AppVar = appVar;
            _isComObject = !AppVar.IsNull && (string)AppVar["GetType"]()["FullName"]().Core == "System.__ComObject";
        }

        protected override AppVar Invoke(MethodInfo method, string name, object[] args, Async async, OperationTypeInfo typeInfo)
        {
            if (_isComObject && typeInfo == null)
            {
                var argTypes = new List<string>();
                foreach (var e in method.GetParameters())
                {
                    argTypes.Add(e.ParameterType.FullName);
                }
                typeInfo = new OperationTypeInfo(method.DeclaringType.FullName, argTypes.ToArray());
                return FriendlyInvokeSpec.GetFriendlyOperation(AppVar, name, async, typeInfo)(args);
            }
            else
            {
                return FriendlyInvokeSpec.GetFriendlyOperation(AppVar, name, async, typeInfo)(args);
            }
        }

        protected override string GetTargetTypeFullName()
        {
            return TargetTypeUtility.GetFullName(App, typeof(TInterface));
        }

        public Async AsyncNext()
        {
            Async async = new Async();
            SetAsyncNext(async);
            return async;
        }
    }
}
