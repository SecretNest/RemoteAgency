using System;
using System.Collections.Generic;
using System.Text;

namespace SecretNest.RemoteAgency.Attributes
{  
    /// <summary>
    /// Specifies the class contains the property to describe whether the element is ignored.
    /// </summary>
    public interface IIsIgnored
    {
        /// <summary>
        /// Gets whether the element is ignored.
        /// </summary>
        bool IsIgnored { get; }
    }
}
