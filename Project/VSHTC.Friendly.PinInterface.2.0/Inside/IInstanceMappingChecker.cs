using System;
using System.Collections.Generic;

namespace VSHTC.Friendly.PinInterface.Inside
{
    static class IInstanceMappingChecker
    {
        internal static Type GetSrcType(Type iinstance)
        {
            var baseInterfaceCheck = new List<Type>();
            bool hasIInstance = false;
            Type maxInterfaceOwner = null;
            int maxInterfaceCount = -1;
            List<Type> iinstanceBase = new List<Type>(typeof(IInstance).GetInterfaces());
            foreach (var i in iinstance.GetInterfaces())
            {
                if (i == typeof(IInstance))
                {
                    hasIInstance = true;
                }
                else if (iinstanceBase.Contains(i))
                {
                    continue;
                }
                else
                {
                    var baseInterfaces = i.GetInterfaces();
                    baseInterfaceCheck.Add(i);
                    if (maxInterfaceCount == baseInterfaces.Length)
                    {
                        throw new NotSupportedException();
                    }
                    else if (maxInterfaceCount < baseInterfaces.Length)
                    {
                        maxInterfaceCount = baseInterfaces.Length;
                        maxInterfaceOwner = i;
                    }
                }
            }
            if (!hasIInstance)
            {
                throw new NotSupportedException();
            }
            if (maxInterfaceOwner == null)
            {
                throw new NotSupportedException();
            }

            baseInterfaceCheck.Remove(maxInterfaceOwner);

            var bases = new List<Type>(maxInterfaceOwner.GetInterfaces());
            foreach (var i in baseInterfaceCheck)
            {
                if (!bases.Contains(i))
                {
                    throw new NotSupportedException();
                }
            }
            return maxInterfaceOwner;
        }

        internal static void CheckGenerateIInstancePlus(Type srcType)
        {
            if (!srcType.IsInterface)
            {
                throw new NotSupportedException();
            }
            if (!srcType.IsPublic && !srcType.IsNestedPublic)
            {
                throw new NotSupportedException();
            }
        }
    }
}
