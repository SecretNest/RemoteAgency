using System;
using System.Collections.Generic;
using System.Text;

namespace SecretNest.RemoteAgency.Attributes
{
    /// <summary>
    /// The base class of all attributes those describe the serializing behaviors.
    /// </summary>
    /// <remarks>All metadata related to derived classes from this attribute will be transferred to the serializer helper for reference when generating code of entities.</remarks>
    public abstract class SerializingBehaviorAttributeBase : Attribute
    {
    }
}
