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
        public static RemoteAgency<string> CreateWithJsonSerializer(Guid? siteId = null)
        {
            return (RemoteAgency<string>)CreateWithoutCheck(new RemoteAgencyJsonSerializer(), new RemoteAgencyJsonSerializerEntityTypeBuilder(), siteId);
        }
    }
}
