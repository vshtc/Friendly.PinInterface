using Codeer.Friendly;
using System;
using System.Collections.Generic;
using VSHTC.Friendly.PinInterface.Inside;

namespace VSHTC.Friendly.PinInterface.Inside
{
    static class TargetTypeUtility
    {
        internal static string GetFullName(AppFriend app, Type type)
        {
            TargetTypeAttribute attr = GetTargetTypeAttribute(type);
            if (attr == null)
            {
                if (TypeUtility.HasInterface(type, typeof(IInstance)) ||
                    TypeUtility.HasInterface(type, typeof(IStatic)) ||
                    TypeUtility.HasInterface(type, typeof(IConstructor)))
                {
                    return string.Empty;
                }
                else
                {
                    return type.FullName;
                }
            }
            else
            {
                if (IsGeneric(attr.FullName))
                {
                    List<string> genericArguments = new List<string>();
                    foreach (var element in type.GetGenericArguments())
                    {
                        string innerType = GetFullName(app, element);
                        if (string.IsNullOrEmpty(innerType))
                        {
                            return string.Empty;
                        }
                        genericArguments.Add(innerType);
                    }
                    return MakeGenericTypeFullName(app, attr.FullName, genericArguments.ToArray());
                }
                else if (IsArray(attr.FullName))
                {
                    var innerTypes = type.GetGenericArguments();
                    if (innerTypes.Length != 1)
                    {
                        return string.Empty;
                    }
                    string arrayCoreType = GetFullName(app, innerTypes[0]);
                    if (string.IsNullOrEmpty(arrayCoreType))
                    {
                        return string.Empty;
                    }
                    return arrayCoreType + attr.FullName;
                }
                else
                {
                    return attr.FullName;
                }
            }
        }

        static bool IsArray(string targetTypeFullName)
        {
            return (targetTypeFullName.IndexOf("[") == 0);
        }

        static bool IsGeneric(string targetTypeFullName)
        {
            return targetTypeFullName.Contains("`");
        }
       
        static string MakeGenericTypeFullName(AppFriend app, string genericTypeFullName, string[] genericArgumentsFullName)
        {
            AppVar typeFinder = app.Dim(new NewInfo("Codeer.Friendly.DotNetExecutor.TypeFinder"));//magic name.
            AppVar genericType = typeFinder["GetType"](genericTypeFullName);
            AppVar genericArguments = app.Dim(new Type[genericArgumentsFullName.Length]);
            for (int i = 0; i < genericArgumentsFullName.Length; i++)
            {
                genericArguments["[]"](i, typeFinder["GetType"](genericArgumentsFullName[i]));
            }
            AppVar type = genericType["MakeGenericType"](genericArguments);
            return (string)type["FullName"]().Core;
        }

        static TargetTypeAttribute GetTargetTypeAttribute(Type interfaceType)
        {
            while (interfaceType != null)
            {
                var attrs = interfaceType.GetCustomAttributes(typeof(TargetTypeAttribute), false);
                if (attrs.Length == 1)
                {
                    return (TargetTypeAttribute)attrs[0];
                }
                interfaceType = interfaceType.DeclaringType;
            }
            return null;
        }
    }
}
