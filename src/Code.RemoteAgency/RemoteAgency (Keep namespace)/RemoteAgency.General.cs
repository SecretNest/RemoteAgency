using System;
using System.Collections.Generic;
using System.Text;

namespace SecretNest.RemoteAgency
{
    /// <summary>
    /// Remote Agency from SecretNest.info. 
    /// </summary>
    /// <typeparam name="TSerialized">Type of the serialized data.</typeparam>
    /// <remarks>Type of the parent class of all entities is set to object.</remarks>
    public partial class RemoteAgency<TSerialized> : RemoteAgency<TSerialized, object>
    {
        /// <summary>
        /// Initializes an instance of Remote Agency with type of serialized data specified.
        /// </summary>
        /// <param name="serializingHelper">Serializer helper.</param>
        /// <param name="entityTypeBuilder">Entity type builder.</param>
        /// <param name="siteId">Site id. A randomized value is used when it is set to <see cref="Guid"/>.Empty.</param>
        /// <remarks>Type of the parent class of all entities is set to object.</remarks>
        public RemoteAgency(SerializingHelperBase<TSerialized, object> serializingHelper, EntityTypeBuilderBase entityTypeBuilder, Guid siteId) : base(serializingHelper, entityTypeBuilder, siteId)
        {
        }
    }
}
