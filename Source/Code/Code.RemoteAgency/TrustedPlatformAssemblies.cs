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
        Dictionary<string, string> shortNameMapping = new Dictionary<string, string>();

        public TrustedPlatformAssemblies()
        {
            string[] locationsOfAllReferences = AppContext.GetData("TRUSTED_PLATFORM_ASSEMBLIES").ToString().Split(';');

            foreach (var file in locationsOfAllReferences)
            {
                var name = System.Runtime.Loader.AssemblyLoadContext.GetAssemblyName(file);
                var fullName = name.FullName;
                var shortName = name.Name;
                var lazy = new Lazy<byte[]>(() => File.ReadAllBytes(file), false);
                platformAssemblies.Add(fullName, lazy);
                if (!shortNameMapping.TryAdd(shortName, fullName))
                {
                    shortNameMapping[shortName] = null;
                }
                if (shortName == "Microsoft.CSharp")
                    CSharpAssemblyName = name;
                //else if (fullname.StartsWith("System.Runtime,"))
                //    RuntimeAssemblyName = name;
                //else if (fullname.StartsWith("System.Collections,"))
                //    CollectionsAssemblyName = name;
                else if (shortName == "System.Linq.Expressions")
                    LinqExpressionsAssemblyName = name;
            }
        }

        public byte[] LoadAssembly(AssemblyName assemblyName)
        {
            if (shortNameMapping.TryGetValue(assemblyName.Name, out string fullName))
            {
                if (fullName == null)
                {
                    fullName = assemblyName.FullName;
                }
                return platformAssemblies[fullName].Value;
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
