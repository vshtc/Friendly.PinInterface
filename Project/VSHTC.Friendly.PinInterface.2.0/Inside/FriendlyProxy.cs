using System;
using System.Reflection;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Remoting.Proxies;
using Codeer.Friendly;
using VSHTC.Friendly.PinInterface.Properties;

namespace VSHTC.Friendly.PinInterface.Inside
{
    abstract class FriendlyProxy<TInterface> : RealProxy, IModifyOperationTypeInfo
    {
        Async _asyncNext;
        OperationTypeInfo _operationTypeInfoNext;
        bool _isAutoOperationTypeInfoNext;

        protected AppFriend App { get; private set; }

        public FriendlyProxy(AppFriend app)
            : base(typeof(TInterface)) 
        {
            App = app;
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

            bool isAsync = _asyncNext != null;

            ArgumentResolver args = new ArgumentResolver(App, method, isAsync, mm.Args);

            if (_operationTypeInfoNext == null)
            {
                if (_isAutoOperationTypeInfoNext || IsNeedAutoOperationTypeInfo)
                {
                    _isAutoOperationTypeInfoNext = false;
                    _operationTypeInfoNext = TargetTypeUtility.TryCreateOperationTypeInfo(App, GetTargetTypeFullName(method), method);
                    if (_operationTypeInfoNext == null)
                    {
                        throw new NotSupportedException(Resources.ErrorGuessOperationTypeInfo);
                    }
                }
            }

            string invokeName = FriendlyInvokeSpec.GetInvokeName(method);
            AppVar returnedAppVal = null;
            var tempAsync = _asyncNext;
            var tempOpe = _operationTypeInfoNext;
            _asyncNext = null;
            _operationTypeInfoNext = null;
            returnedAppVal = Invoke(method, invokeName, args.InvokeArguments, tempAsync, tempOpe);

            //Resolve return value and ref out arguments.
            object objReturn = ReturnResolver.Resolve(isAsync, returnedAppVal, method.ReturnParameter);
            var refoutArgs = args.GetRefOutArgs();
            return new ReturnMessage(objReturn, refoutArgs, refoutArgs.Length, mm.LogicalCallContext, (IMethodCallMessage)msg);
        }

        public void OperationTypeInfoNext(OperationTypeInfo info)
        {
            _operationTypeInfoNext = info;
        }

        public void SetOperationTypeInfoNextAuto()
        {
            _isAutoOperationTypeInfoNext = true;
        }

        protected void SetAsyncNext(Async asyncNext)
        {
            _asyncNext = asyncNext;
        }

        protected virtual bool IsNeedAutoOperationTypeInfo { get { return false; } }
        protected abstract string GetTargetTypeFullName(MethodInfo method);
        protected abstract AppVar Invoke(MethodInfo method, string name, object[] args, Async async, OperationTypeInfo info);
    }
}