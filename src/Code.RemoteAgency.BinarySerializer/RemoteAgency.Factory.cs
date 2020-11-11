using System;
using System.Collections.Generic;
using System.Text;
using SecretNest.RemoteAgency.BinarySerializer;

namespace SecretNest.RemoteAgency
{
    abstract partial class RemoteAgencyBase
    {
        /// <summary>
        /// Creates an instance of Remote Agency using binary serializer.
        /// </summary>
        /// <param name="securityIssueAcknowledged">This value needs to be set to <see langword="true" /> to confirm that the security issue is known, or an exception will be thrown. Default value is <see langword="false"/>.</param>
        /// <param name="siteId">Site id. A randomized value is used when it is set to <see cref="Guid"/>.Empty or absent.</param>
        /// <returns>Created Remote Agency instance.</returns>
        /// <note type="warning">BinaryFormatter is included in this serializer, which is dangerous and should not be used. See <conceptualLink target="886b6555-5b60-46ed-b0e3-aa383c95108c" >Binary Formatter Warning</conceptualLink> for details.</note>
        public static RemoteAgency<byte[]> CreateWithBinarySerializer(bool securityIssueAcknowledged = false, Guid? siteId = null)
        {
            return (RemoteAgency<byte[]>)CreateWithoutCheck<byte[], object>(new RemoteAgencyBinarySerializer(securityIssueAcknowledged), new RemoteAgencyBinarySerializerEntityTypeBuilder(), siteId);
        }
    }
}
