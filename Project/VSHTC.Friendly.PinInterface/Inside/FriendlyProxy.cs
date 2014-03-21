using System;
using System.Reflection;
using System.Runtime.Remoting.Messaging;
using Codeer.Friendly;
using System.Runtime.Remoting.Proxies;

namespace VSHTC.Friendly.PinInterface.Inside
{
    abstract class FriendlyProxy<TInterface> : RealProxy
    {
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

            var returnedAppVal = Invoke(method, mm.Args);

            object objReturn = ToReturnObject(returnedAppVal, method.ReturnParameter);
            return new ReturnMessage(objReturn, null, 0, mm.LogicalCallContext, (IMethodCallMessage)msg);
        }

        protected abstract AppVar Invoke(MethodInfo method, object[] args);

        private object ToReturnObject(AppVar returnedAppVal, ParameterInfo parameterInfo)
        {
            if (parameterInfo.ParameterType == typeof(void))
            {
                return null;
            }
            else if (parameterInfo.ParameterType == typeof(AppVar))
            {
                return returnedAppVal;
            }
            else if (parameterInfo.ParameterType.IsInterface)
            {
                return WrapChildAppVar(parameterInfo.ParameterType, returnedAppVal);
            }
            else
            {
                return returnedAppVal.Core;
            }
        }

        private object WrapChildAppVar(Type type, AppVar ret)
        {
            var friendlyProxyType = typeof (FriendlyProxyInstance<>).MakeGenericType(type);
            dynamic friendlyProxy = Activator.CreateInstance(friendlyProxyType, new object[] {App, ret});
            return friendlyProxy.GetTransparentProxy();
        }
    }
}