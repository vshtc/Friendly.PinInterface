using System;

namespace VSHTC.Friendly.PinInterface
{
    public class ProxyTragetAttribute : Attribute
    {
        public ProxyTragetAttribute(string targetTypeFullName)
        {
            TargetTypeFullName = targetTypeFullName;
        }
        public string TargetTypeFullName { get; set; }
    }
}
