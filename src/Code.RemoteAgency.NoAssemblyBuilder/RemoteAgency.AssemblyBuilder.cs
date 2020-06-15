using System;
using System.Collections.Generic;
using System.Text;

namespace SecretNest.RemoteAgency
{
    public abstract partial class RemoteAgency
    {
        bool BuildAssembly(string assemblyName, IEnumerable<string> sourceCode, IEnumerable<AssemblyReference> references, out byte[] assemblyImage, out TypeCreatingException buidlingError)
        {
            throw new NotSupportedException("Neat version of RemoteAgency does not shipped with built-in assembly builder.");
        }
    }
}
