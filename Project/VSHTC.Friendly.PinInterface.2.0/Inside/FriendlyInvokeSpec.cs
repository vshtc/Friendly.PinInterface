using System.Text;
using System.Reflection;
using Codeer.Friendly;

namespace VSHTC.Friendly.PinInterface.Inside
{
    class FriendlyInvokeSpec
    {
        internal static string GetInvokeName(MethodInfo method)
        {
            //Treat array and other index access equally. 
            string invokeName = method.Name;
            if (invokeName == "get_Item")
            {
                return "[" + GetCommas(method.GetParameters().Length - 1) + "]";
            }
            else if (invokeName == "set_Item")
            {
                return "[" + GetCommas(method.GetParameters().Length - 2) + "]";
            }
            //Treat property and field equally. 
            else if (invokeName.IndexOf("get_") == 0 ||
                    invokeName.IndexOf("set_") == 0)
            {
                return invokeName.Substring(4);
            }
            return invokeName;
        }

        internal static FriendlyOperation GetFriendlyOperation(AppVar target, string name, Async async, OperationTypeInfo typeInfo)
        {
            if (async != null && typeInfo != null)
            {
                return target[name, typeInfo, async];
            }
            else if (async != null)
            {
                return target[name, async];
            }
            else if (typeInfo != null)
            {
                return target[name, typeInfo];
            }
            return target[name];
        }

        internal static FriendlyOperation GetFriendlyOperation(AppFriend target, string name, Async async, OperationTypeInfo typeInfo)
        {
            if (async != null && typeInfo != null)
            {
                return target[name, typeInfo, async];
            }
            else if (async != null)
            {
                return target[name, async];
            }
            else if (typeInfo != null)
            {
                return target[name, typeInfo];
            }
            return target[name];
        }

        static string GetCommas(int count)
        {
            StringBuilder b = new StringBuilder();
            for (int i = 0; i < count; i++)
            {
                b.Append(",");
            }
            return b.ToString();
        }
    }
}
