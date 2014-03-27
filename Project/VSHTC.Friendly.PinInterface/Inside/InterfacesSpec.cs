using Codeer.Friendly;
using System.Reflection;
using VSHTC.Friendly.PinInterface.FunctionalInterfaces;

namespace VSHTC.Friendly.PinInterface.Inside
{
    static class InterfacesSpec
    {
        internal static bool IsGetType(MethodInfo method)
        {
            return (method.DeclaringType == typeof(object) && method.Name == "GetType");
        }
        internal static bool IsAppVar(MethodInfo method)
        {
            return (method.DeclaringType == typeof(IAppVarOwner) && method.Name == "get_AppVar");
        }
        internal static bool IsAsyncNext(MethodInfo method)
        {
            return (method.DeclaringType == typeof(IModifyAsync) && method.Name == "AsyncNext");
        }
        internal static bool IsOperationTypeInfoNext(MethodInfo method)
        {
            return (method.DeclaringType == typeof(IModifyOperationTypeInfo) && method.Name == "OperationTypeInfoNext");
        }
    }
}
