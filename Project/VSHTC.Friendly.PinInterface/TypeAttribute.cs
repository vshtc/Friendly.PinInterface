using System;

namespace VSHTC.Friendly.PinInterface
{
    public class TypeAttribute : Attribute
    {
        public TypeAttribute(string targetTypeFullName)
        {
            TargetTypeFullName = targetTypeFullName;
        }
        public string TargetTypeFullName { get; set; }
    }
}
