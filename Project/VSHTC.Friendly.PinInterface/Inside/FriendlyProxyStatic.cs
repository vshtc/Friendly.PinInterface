using Codeer.Friendly;
using System;
using System.Linq;
using System.Reflection;

namespace VSHTC.Friendly.PinInterface.Inside
{
    class FriendlyProxyStatic<TInterface> : FriendlyProxy<TInterface>
        where TInterface : IStatic
    {
        string _typeFullName;

        public FriendlyProxyStatic(AppFriend app, string typeFullName)
            : base(app)
        {
            _typeFullName = typeFullName;
        }

        protected override AppVar Invoke(MethodInfo method, string name, object[] args, ref Async async, ref OperationTypeInfo typeInfo)
        {
            try
            {
                if (method.GetCustomAttributes(true).Any(e => e.GetType() == typeof(ConstructorAttribute)))
                {
                    if (async != null)
                    {
                        throw new NotSupportedException("オブジェクト生成にAsyncを使用することはできません。");
                    }
                    return (typeInfo == null) ?
                        App.Dim(new NewInfo(_typeFullName, args)) :
                        App.Dim(new NewInfo(_typeFullName, args), typeInfo);
                }
                return GetFriendlyOperation(App, _typeFullName + "." + name, async, typeInfo)(args);
            }
            finally
            {
                async = null;
                typeInfo = null;
            }
        }
    }
}
