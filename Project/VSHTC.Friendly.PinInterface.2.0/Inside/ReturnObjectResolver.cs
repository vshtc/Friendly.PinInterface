using Codeer.Friendly;
using System.Reflection;

namespace VSHTC.Friendly.PinInterface.Inside
{
    static class ReturnObjectResolver
    {
        internal static object Resolve(bool isAsync, AppVar returnedAppVal, ParameterInfo parameterInfo)
        {
            if (parameterInfo.ParameterType == typeof(void))
            {
                return null;
            }
            else if (parameterInfo.ParameterType.IsInterface)
            {
                return FriendlyProxyFactory.WrapFriendlyProxyInstance(parameterInfo.ParameterType, returnedAppVal);
            }
            else
            {
                return isAsync ? TypeUtility.GetDefault(parameterInfo.ParameterType) : returnedAppVal.Core;
            }
        }
    }
}
