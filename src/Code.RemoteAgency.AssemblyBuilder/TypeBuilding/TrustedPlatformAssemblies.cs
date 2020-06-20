#if !net461

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Reflection;
using System.Text;

namespace SecretNest.RemoteAgency.TypeBuilding
{
    class TrustedPlatformAssemblies
    {

        readonly Dictionary<string, Lazy<byte[]>> _platformAssemblies = new Dictionary<string, Lazy<byte[]>>();
        readonly Dictionary<string, string> _shortNameMapping = new Dictionary<string, string>();

        [SuppressMessage("ReSharper", "PossibleNullReferenceException")]
        public TrustedPlatformAssemblies()
        {
            string[] locationsOfAllReferences = AppContext.GetData("TRUSTED_PLATFORM_ASSEMBLIES").ToString().Split(';');

            foreach (var file in locationsOfAllReferences)
            {
                var name = AssemblyName.GetAssemblyName(file);
                var fullName = name.FullName;
                var shortName = name.Name;
                var lazy = new Lazy<byte[]>(() => File.ReadAllBytes(file), false);
                _platformAssemblies.Add(fullName, lazy);
                if (_shortNameMapping.ContainsKey(shortName))
                {
                    _shortNameMapping[shortName] = null;
                }
                else
                {
                    _shortNameMapping[shortName] = fullName;
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
            if (_shortNameMapping.TryGetValue(assemblyName.Name, out string fullName))
            {
                if (fullName == null)
                {
                    fullName = assemblyName.FullName;
                }
                return _platformAssemblies[fullName].Value;
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

    }
}
#endif
