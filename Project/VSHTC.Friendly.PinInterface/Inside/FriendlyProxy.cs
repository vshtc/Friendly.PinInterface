using System;
using System.Reflection;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Remoting.Proxies;
using Codeer.Friendly;

namespace VSHTC.Friendly.PinInterface.Inside
{
    abstract class FriendlyProxy<TInterface> : RealProxy
    {
        Async _asyncNext;
        OperationTypeInfo _operationTypeInfoNext;

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

            //特殊インターフェイス対応
            object retunObject;
            if (InterfacesSpec.TryExecuteSpecialInterface<TInterface>(this as IAppVarOwner, method, mm.Args, 
                ref _asyncNext, ref _operationTypeInfoNext, out retunObject))
            {
                return new ReturnMessage(retunObject, null, 0, mm.LogicalCallContext, (IMethodCallMessage)msg);
            }
            bool isAsyunc = _asyncNext != null;

            //引数解決
            ArgumentResolver args = new ArgumentResolver(App, method, isAsyunc, mm.Args);

            //呼び出し            
            string invokeName = FriendlyInvokeSpec.GetInvokeName(method);
            var returnedAppVal = Invoke(method, invokeName, args.InvokeArguments, ref _asyncNext, ref _operationTypeInfoNext);

            //戻り値とout,refの処理
            object objReturn = ReturnObjectResolver.Resolve(isAsyunc, returnedAppVal, method.ReturnParameter);
            var refoutArgs = args.GetRefOutArgs();
            return new ReturnMessage(objReturn, refoutArgs, refoutArgs.Length, mm.LogicalCallContext, (IMethodCallMessage)msg);
        }

        protected abstract AppVar Invoke(MethodInfo method, string name, object[] args, ref Async async, ref OperationTypeInfo info);
    }
}