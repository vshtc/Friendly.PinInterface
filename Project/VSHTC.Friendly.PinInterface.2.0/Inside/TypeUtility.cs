using System;

namespace VSHTC.Friendly.PinInterface.Inside
{
    static class TypeUtility
    {
        internal static bool HasInterface(Type ownerType, Type inType)
        {
            foreach (var element in ownerType.GetInterfaces())
            {
                if (element == inType)
                {
                    return true;
                }
            }
            return false;
        }

        internal static object GetDefault(Type type)
        {
            IValue value = Activator.CreateInstance(typeof(DefaultValue<>).MakeGenericType(type), new object[] { }) as IValue;
            return value.Value;
        }

        interface IValue
        {
            object Value { get; }
        }
        class DefaultValue<T> : IValue
        {
            public object Value { get { return default(T); } }
        }
    }
}
