using Codeer.Friendly;
using System.Reflection;

namespace VSHTC.Friendly.PinInterface.Inside
{
    static class ReturnObjectResolver
    {
        internal static object Resolve(bool isAsync, AppVar returnedAppVal, ParameterInfo parameterInfo)
        {
            var type = MapIInstance.TryConvertType(parameterInfo.ParameterType);
            if (type == typeof(void))
            {
                return null;
            }
            else if (TypeUtility.HasInterface(type, typeof(IInstance)))
            {
                return FriendlyProxyFactory.WrapFriendlyProxyInstance(type, returnedAppVal);
            }
            else if (type == typeof(AppVar))
            {
                return returnedAppVal;
            }
            else if (UserWrapperUtility.IsAppVarWrapper(type))
            {
                return UserWrapperUtility.CreateWrapper(type, returnedAppVal);
            }
            else
            {
                return isAsync ? TypeUtility.GetDefault(type) : returnedAppVal.Core;
            }
        }
    }
}
