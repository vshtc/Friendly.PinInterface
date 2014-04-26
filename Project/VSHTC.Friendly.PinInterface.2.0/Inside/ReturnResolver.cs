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
            var func = UserWrapperUtility.FindCreateConstructor(type);
            if (func != null)
            {
                return func(returnedAppVal);
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
