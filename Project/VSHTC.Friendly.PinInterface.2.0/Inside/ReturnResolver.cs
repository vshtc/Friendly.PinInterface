using Codeer.Friendly;
using System.Reflection;

namespace VSHTC.Friendly.PinInterface.Inside
{
    static class ReturnResolver
    {
        internal static object Resolve(bool isAsync, AppVar returnedAppVal, ParameterInfo parameterInfo)
        {
            var type = parameterInfo.ParameterType;
            if (type == typeof(void))
            {
                return null;
            }
            else if (type == typeof(AppVar))
            {
                return returnedAppVal;
            }

            if (isAsync)
            {
                return TypeUtility.GetDefault(type);
            }
            
            if (UserWrapperUtility.IsAppVarWrapper(type))
            {
                return UserWrapperUtility.CreateWrapper(type, returnedAppVal);
            }
            else if (type.IsInterface)
            {
                return FriendlyProxyFactory.WrapFriendlyProxyInstance(type, returnedAppVal);
            }
            else
            {
                return returnedAppVal.Core;
            }
        }
    }
}
