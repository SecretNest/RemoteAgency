using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace SecretNest.RemoteAgency
{
    class TrustedPlatformAssemblies
    {
#if netcore

        Dictionary<string, Lazy<byte[]>> platformAssemblies = new Dictionary<string, Lazy<byte[]>>();

        public TrustedPlatformAssemblies()
        {
            string[] locationsOfAllReferences = AppContext.GetData("TRUSTED_PLATFORM_ASSEMBLIES").ToString().Split(';');

            foreach (var file in locationsOfAllReferences)
            {
                var name = System.Runtime.Loader.AssemblyLoadContext.GetAssemblyName(file);
                var fullname = name.FullName;
                var lazy = new Lazy<byte[]>(() => File.ReadAllBytes(file), false);
                platformAssemblies.Add(fullname, lazy);
                if (fullname.StartsWith("Microsoft.CSharp,"))
                    CSharpAssemblyName = name;
                //else if (fullname.StartsWith("System.Runtime,"))
                //    RuntimeAssemblyName = name;
                //else if (fullname.StartsWith("System.Collections,"))
                //    CollectionsAssemblyName = name;
                else if (fullname.StartsWith("System.Linq.Expressions,"))
                    LinqExpressionsAssemblyName = name;
            }
        }

        public byte[] LoadAssembly(string assemblyFullName)
        {
            if (platformAssemblies.TryGetValue(assemblyFullName, out var value))
            {
                return value.Value;
            }
            else
            {
                return null;
            }
        }

        public AssemblyName CSharpAssemblyName { get; private set; }
        //public AssemblyName RuntimeAssemblyName { get; private set; }
        //public AssemblyName CollectionsAssemblyName { get; private set; }
        public AssemblyName LinqExpressionsAssemblyName { get; private set; }
        //#else

        //        public byte[] LoadAssembly(string assemblyFullName)
        //        {
        //            return null;
        //        }

#endif
    }
}
