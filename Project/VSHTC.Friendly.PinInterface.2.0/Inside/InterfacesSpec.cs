using Codeer.Friendly;
using System.Reflection;

namespace VSHTC.Friendly.PinInterface.Inside
{
    static class InterfacesSpec
    {
        internal static bool TryExecute<TInterface>(IAppVarOwner appVarOwner, MethodInfo method, object[] args, out object retunObject)
        {
            retunObject = null;

            //GetType is not proxy.
            //Type may be unable to be serialized. 
            if (IsGetType(method))
            {
                retunObject = typeof(TInterface);
                return true;
            }
            return false;
        }

        static bool IsGetType(MethodInfo method)
        {
            return (method.DeclaringType == typeof(object) && method.Name == "GetType");
        }
    }
}
