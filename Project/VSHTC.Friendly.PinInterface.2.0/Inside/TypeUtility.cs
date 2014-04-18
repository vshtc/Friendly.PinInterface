using System;

namespace VSHTC.Friendly.PinInterface.Inside
{
    static class TypeUtility
    {
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
