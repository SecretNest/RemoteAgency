using System;
using System.Collections.Generic;
using System.Text;

namespace SecretNest.RemoteAgency.AssemblyBuilding
{
    /// <summary>
    /// Contains a list of the Remote Agency built type. Entity classes are excluded.
    /// </summary>
    public enum BuiltClassType
    {
        /// <summary>
        /// Class implemented the interface specified, transferring all accessing from and to the service wrapper.
        /// </summary>
        Proxy,
        /// <summary>
        /// Class working with proxy to forward accessing from and to the real service object.
        /// </summary>
        ServiceWrapper
    }
}
