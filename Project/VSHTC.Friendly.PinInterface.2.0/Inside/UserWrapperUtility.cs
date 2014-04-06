using System;
using Codeer.Friendly;

namespace VSHTC.Friendly.PinInterface.Inside
{
    static class UserWrapperUtility
    {
        internal static bool IsAppVarWrapper(Type type)
        {
            return (type.GetConstructor(new Type[] { typeof(AppVar) }) != null) ||
                //TODO: This is interim procedures until a circumference library catches up. 
                    (type.GetConstructor(new Type[] { typeof(AppFriend), typeof(AppVar) }) != null);
        }

        internal static object CreateWrapper(Type type, AppVar appVar)
        {
            var constructor = type.GetConstructor(new Type[] { typeof(AppVar) });
            if (constructor != null)
            {
                return constructor.Invoke(new object[] { appVar });
            }
            //TODO: This is interim procedures until a circumference library catches up. 
            constructor = type.GetConstructor(new Type[] { typeof(AppVar), typeof(AppVar) });
            if (constructor != null)
            {
                return constructor.Invoke(new object[] { appVar.App, appVar });
            }
            throw new NotSupportedException();
        }
        //TODO: Exception Message
    }
}
