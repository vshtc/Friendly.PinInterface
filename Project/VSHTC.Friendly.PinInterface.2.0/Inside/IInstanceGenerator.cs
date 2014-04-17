using System;
using System.Reflection;
using System.Reflection.Emit;

namespace VSHTC.Friendly.PinInterface.Inside
{
    class IInstanceGenerator
    {
        internal static Type AddIInstance(Type srcType, string uniqueNamespaceForGenerate, int uniqueIndex)
        {
            AssemblyName asmName = new AssemblyName { Name = uniqueNamespaceForGenerate };
            AppDomain domain = AppDomain.CurrentDomain;
            AssemblyBuilder asmBuilder = domain.DefineDynamicAssembly(asmName, AssemblyBuilderAccess.Run);

            ModuleBuilder modBuilder = asmBuilder.DefineDynamicModule(uniqueNamespaceForGenerate);
            TypeBuilder typeBuilder = modBuilder.DefineType(uniqueNamespaceForGenerate + ".I" + uniqueIndex,
                TypeAttributes.Public | TypeAttributes.Interface | TypeAttributes.Abstract, null, new Type[] { srcType, typeof(IInstance) });

            return typeBuilder.CreateType();
        }
    }
}
