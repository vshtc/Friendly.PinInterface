﻿using Codeer.Friendly;
using System.Reflection;
using VSHTC.Friendly.PinInterface.FunctionalInterfaces;

namespace VSHTC.Friendly.PinInterface.Inside
{
    static class InterfacesSpec
    {
        internal static bool TryExecute<TInterface>(IAppVarOwner appVarOwner, MethodInfo method, object[] args,
                                ref Async asyncNext, ref OperationTypeInfo operationTypeInfoNext, ref bool isAutoOperationTypeInfo, out object retunObject)
        {
            retunObject = null;

            //GetType is not proxy.
            //Type may be unable to be serialized. 
            if (IsGetType(method))
            {
                retunObject = typeof(TInterface);
                return true;
            }
            if (IsAsyncNext(method))
            {
                asyncNext = new Async();
                retunObject = asyncNext;
                return true;
            }
            if (IsOperationTypeInfoNext(method))
            {
                operationTypeInfoNext = new OperationTypeInfo((string)args[0], (string[])args[1]);
                return true;
            }
            if (IsSetIsAutoOperationTypeInfo(method))
            {
                isAutoOperationTypeInfo = (bool)args[0];
                return true;
            }
            if (IsGetIsAutoOperationTypeInfo(method))
            {
                retunObject = isAutoOperationTypeInfo;
                return true;
            }
            if (IsAppVar(method))
            {
                retunObject = appVarOwner.AppVar;
                return true;
            }
            return false;
        }

        static bool IsGetType(MethodInfo method)
        {
            return (method.DeclaringType == typeof(object) && method.Name == "GetType");
        }

        static bool IsAppVar(MethodInfo method)
        {
            return (method.DeclaringType == typeof(IAppVarOwner) && method.Name == "get_AppVar");
        }

        static bool IsAsyncNext(MethodInfo method)
        {
            return (method.DeclaringType == typeof(IModifyAsync) && method.Name == "AsyncNext");
        }

        static bool IsOperationTypeInfoNext(MethodInfo method)
        {
            return (method.DeclaringType == typeof(IModifyOperationTypeInfo) && method.Name == "OperationTypeInfoNext");
        }

        static bool IsSetIsAutoOperationTypeInfo(MethodInfo method)
        {
            return (method.DeclaringType == typeof(IModifyOperationTypeInfo) && method.Name == "set_IsAutoOperationTypeInfo");
        }

        static bool IsGetIsAutoOperationTypeInfo(MethodInfo method)
        {
            return (method.DeclaringType == typeof(IModifyOperationTypeInfo) && method.Name == "get_IsAutoOperationTypeInfo");
        }

        //@@@自動でONになる属性も追加
    }
}
