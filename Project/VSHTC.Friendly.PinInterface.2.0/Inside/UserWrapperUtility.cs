using System;
using Codeer.Friendly;
using System.Reflection;

namespace VSHTC.Friendly.PinInterface.Inside
{
    static class UserWrapperUtility
    {
        internal delegate object Construct(AppVar appVar);

        internal static Construct FindCreateConstructor(Type type)
        {
            foreach (var element in type.GetConstructors(BindingFlags.Public | BindingFlags.Instance))
            {
                var args = element.GetParameters();
                if (args.Length == 1 && args[0].ParameterType.IsAssignableFrom(typeof(AppVar)))
                {
                    var hitConstructor = element;
                    return (v) => AppVarUtility.IsNull(v) ? null :  hitConstructor.Invoke(new object[] { v });
                }
            }
            return null;
        }
    }
}
