using System;
using System.Reflection;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Remoting.Proxies;
using Codeer.Friendly;

namespace VSHTC.Friendly.PinInterface.Inside
{
    abstract class FriendlyProxy<TInterface> : RealProxy, IModifyOperationTypeInfo
    {
        Async _asyncNext;
        OperationTypeInfo _operationTypeInfoNext;
        bool _isAutoOperationTypeInfoNext;
        bool _isAutoOperationTypeInfoAlways;

        protected AppFriend App { get; private set; }

        public void OperationTypeInfoNext(OperationTypeInfo info)
        {
            _operationTypeInfoNext = info;
        }

        protected void SetAsyncNext(Async asyncNext)
        {
            _asyncNext = asyncNext;
        }

        public void SetOperationTypeInfoNextAuto()
        {
            _isAutoOperationTypeInfoNext = true;
        }

        public void SetOperationTypeInfoAutoAlways(bool always)
        {
            _isAutoOperationTypeInfoAlways = always;
        }

        public FriendlyProxy(AppFriend app)
            : base(typeof(TInterface)) 
        {
            App = app;
            _isAutoOperationTypeInfoAlways = 
                0 < typeof(TInterface).GetCustomAttributes(typeof(OperationTypeInfoAutoAttribute), false).Length;
        }

        public override IMessage Invoke(IMessage msg)
        {
            var mm = msg as IMethodMessage;
            var method = (MethodInfo)mm.MethodBase;

            object retunObject;
            if (InterfacesSpec.TryExecute<TInterface>(this as IAppVarOwner, method, mm.Args, out retunObject))
            {
                return new ReturnMessage(retunObject, null, 0, mm.LogicalCallContext, (IMethodCallMessage)msg);
            }
            bool isAsyunc = _asyncNext != null;

            ArgumentResolver args = new ArgumentResolver(App, method, isAsyunc, mm.Args);

            if (_operationTypeInfoNext == null)
            {
                if (_isAutoOperationTypeInfoNext || 
                    _isAutoOperationTypeInfoAlways)
                {
                    _operationTypeInfoNext = TargetTypeUtility.TryCreateOperationTypeInfo(App, GetTargetTypeFullName(), method);
                    _isAutoOperationTypeInfoNext = false;
                }
            }

            string invokeName = FriendlyInvokeSpec.GetInvokeName(method);
            AppVar returnedAppVal = null;
            try
            {
                returnedAppVal = Invoke(method, invokeName, args.InvokeArguments, _asyncNext, _operationTypeInfoNext);
            }
            finally
            {
                _asyncNext = null;
                _operationTypeInfoNext = null;
            }

            //Resolve return value and ref out arguments.
            object objReturn = ReturnResolver.Resolve(isAsyunc, returnedAppVal, method.ReturnParameter);
            var refoutArgs = args.GetRefOutArgs();
            return new ReturnMessage(objReturn, refoutArgs, refoutArgs.Length, mm.LogicalCallContext, (IMethodCallMessage)msg);
        }

        protected abstract string GetTargetTypeFullName();
        protected abstract AppVar Invoke(MethodInfo method, string name, object[] args, Async async, OperationTypeInfo info);
    }
}