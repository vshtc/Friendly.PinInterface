using System;
using Codeer.Friendly;
using System.Reflection;

namespace VSHTC.Friendly.PinInterface.Inside
{
    static class UserWrapperUtility
    {
        internal static bool IsAppVarWrapper(Type type)
        {
            foreach (var element in type.GetConstructors(BindingFlags.Public | BindingFlags.Instance))
            {
                var args = element.GetParameters();
                if (args.Length == 1 && args[0].ParameterType.IsAssignableFrom(typeof(AppVar)))
                {
                    return true;
                }
            }
            return false;
        }

        internal static object CreateWrapper(Type type, AppVar appVar)
        {
            if (AppVarUtility.IsNull(appVar))
            {
                return null;
            }

            foreach (var element in type.GetConstructors(BindingFlags.Public | BindingFlags.Instance))
            {
                var args = element.GetParameters();
                if (args.Length == 1 && args[0].ParameterType.IsAssignableFrom(typeof(AppVar)))
                {
                    return element.Invoke(new object[] { appVar });
                }
            } 
            throw new NotSupportedException();
        }
    }
}
