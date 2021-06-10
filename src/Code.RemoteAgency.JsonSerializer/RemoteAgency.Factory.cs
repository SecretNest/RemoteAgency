using System;
using SecretNest.RemoteAgency.JsonSerializer;

namespace SecretNest.RemoteAgency
{
    abstract partial class RemoteAgencyBase
    {
        /// <summary>
        /// Creates an instance of Remote Agency using Json serializer.
        /// </summary>
        /// <param name="siteId">Site id. A randomized value is used when it is set to <see cref="Guid"/>.Empty or absent.</param>
        /// <returns>Created Remote Agency instance.</returns>
        /// <remarks><para>This method is not present in Neat release.</para></remarks>
        public static RemoteAgency<string, object> CreateWithJsonSerializer(Guid? siteId = null)
        {
            return CreateWithoutCheck(new RemoteAgencyJsonSerializer(), new RemoteAgencyJsonSerializerEntityTypeBuilder(), siteId);
        }
    }
}
