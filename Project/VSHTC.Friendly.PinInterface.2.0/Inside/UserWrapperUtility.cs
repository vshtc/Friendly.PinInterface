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
                //TODO: This is interim procedures until a circumference library catches up. 
                if (args.Length == 2 && 
                    typeof(AppFriend).IsAssignableFrom(args[0].ParameterType) &&
                    args[1].ParameterType.IsAssignableFrom(typeof(AppVar)))
                {
                    return true;
                }
            }
            return false;
        }

        internal static object CreateWrapper(Type type, AppVar appVar)
        {
            if ((bool)appVar.App[typeof(object), "ReferenceEquals"](null, appVar).Core)
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
                //TODO: This is interim procedures until a circumference library catches up. 
                if (args.Length == 2 &&
                    typeof(AppFriend).IsAssignableFrom(args[0].ParameterType) &&
                    args[1].ParameterType.IsAssignableFrom(typeof(AppVar)))
                {
                    return element.Invoke(new object[] { appVar.App, appVar });
                }
            } 
            throw new NotSupportedException();
        }
        //TODO: Exception Message
    }
}
