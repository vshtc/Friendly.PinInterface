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

            object retunObject;
            if (FriendlyProxyUtiltiy.TryExecuteSpecialInterface<TInterface>(method, mm.Args, ref _asyncNext, ref _operationTypeInfoNext, out retunObject))
            {
                return new ReturnMessage(retunObject, null, 0, mm.LogicalCallContext, (IMethodCallMessage)msg);
            }
            bool isAsyunc = _asyncNext != null;

            //out refëŒâû
            object[] args;
            Func<object>[] refoutArgsFunc;
            FriendlyProxyUtiltiy.AdjustRefOutArgs(App, method, isAsyunc, mm.Args, out args, out refoutArgsFunc);

            //åƒÇ—èoÇµ            
            string invokeName = FriendlyProxyUtiltiy.GetInvokeName(method);
            var returnedAppVal = Invoke(method, invokeName, args, ref _asyncNext, ref _operationTypeInfoNext);

            //ñﬂÇËílÇ∆out,refÇÃèàóù
            object objReturn = FriendlyProxyUtiltiy.ToReturnObject(isAsyunc, returnedAppVal, method.ReturnParameter);
            var refoutArgs = FriendlyProxyUtiltiy.ToRefOutArgs(refoutArgsFunc);
            return new ReturnMessage(objReturn, refoutArgs, refoutArgs.Length, mm.LogicalCallContext, (IMethodCallMessage)msg);
        }

        protected abstract AppVar Invoke(MethodInfo method, string name, object[] args, ref Async async, ref OperationTypeInfo info);
    }
}