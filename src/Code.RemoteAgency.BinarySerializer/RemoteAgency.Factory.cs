using System;
using SecretNest.RemoteAgency.BinarySerializer;

namespace SecretNest.RemoteAgency
{
    abstract partial class RemoteAgencyBase
    {
#if NET9_0_OR_GREATER
        /// <summary>
        /// Obsoleted method. This method is not present from .net 9 version releases. Creates an instance of Remote Agency using binary serializer.
        /// </summary>
        /// <param name="securityIssueAcknowledged">This value needs to be set to <see langword="true" /> to confirm that the security issue is known, or an exception will be thrown. Default value is <see langword="false"/>.</param>
        /// <param name="siteId">Site id. A randomized value is used when it is set to <see cref="Guid"/>.Empty or absent.</param>
        /// <returns>Created Remote Agency instance.</returns>
        /// <note type="warning">BinaryFormatter is included in this serializer, which is dangerous and should not be used. See <conceptualLink target="886b6555-5b60-46ed-b0e3-aa383c95108c" >Binary Formatter Warning</conceptualLink> for details.</note>
        /// <remarks><para>This method is not present in Neat release.</para></remarks>
        [Obsolete("This method is not present from .net 9 version releases.")]
        public static RemoteAgency<byte[], object> CreateWithBinarySerializer(bool securityIssueAcknowledged = false, Guid? siteId = null)
        {
            throw new NotSupportedException();
        }
#else
        /// <summary>
        /// Creates an instance of Remote Agency using binary serializer.
        /// </summary>
        /// <param name="securityIssueAcknowledged">This value needs to be set to <see langword="true" /> to confirm that the security issue is known, or an exception will be thrown. Default value is <see langword="false"/>.</param>
        /// <param name="siteId">Site id. A randomized value is used when it is set to <see cref="Guid"/>.Empty or absent.</param>
        /// <returns>Created Remote Agency instance.</returns>
        /// <note type="warning">BinaryFormatter is included in this serializer, which is dangerous and should not be used. See <conceptualLink target="886b6555-5b60-46ed-b0e3-aa383c95108c" >Binary Formatter Warning</conceptualLink> for details.</note>
        /// <remarks><para>This method is not present in Neat release.</para><para>This class is not present from .net 9 version releases.</para></remarks>
        public static RemoteAgency<byte[], object> CreateWithBinarySerializer(bool securityIssueAcknowledged = false, Guid? siteId = null)
        {
            return CreateWithoutCheck(new RemoteAgencyBinarySerializer(securityIssueAcknowledged), new RemoteAgencyBinarySerializerEntityTypeBuilder(), siteId);
        }
#endif
    }
}
