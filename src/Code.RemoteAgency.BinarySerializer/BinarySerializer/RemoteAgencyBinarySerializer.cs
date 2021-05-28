using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security;

namespace SecretNest.RemoteAgency.BinarySerializer
{
    /// <summary>
    /// Provides binary based serializing and deserializing methods for entities.
    /// </summary>
    /// <note type="warning">BinaryFormatter is included in this serializer, which is dangerous and should not be used. See <conceptualLink target="886b6555-5b60-46ed-b0e3-aa383c95108c" >Binary Formatter Warning</conceptualLink> for details.</note>
    public class RemoteAgencyBinarySerializer: SerializingHelperBase<byte[], object>
    {
        readonly BinaryFormatter _formatter;

        /// <summary>
        /// Initializes an instance of RemoteAgencyBinarySerializer.
        /// </summary>
        /// <param name="securityIssueAcknowledged">This value needs to be set to <see langword="true" /> to confirm that the security issue is known, or an exception will be thrown. Default value is <see langword="false"/>.</param>
        public RemoteAgencyBinarySerializer(bool securityIssueAcknowledged = false)
        {
            if (!securityIssueAcknowledged)
                throw new SecurityException("BinaryFormatter is included in this serializer, which is dangerous and should not be used.");
            _formatter = new BinaryFormatter();
        }

        /// <inheritdoc />
        public override byte[] Serialize(object original)
        {
            using var stream = new MemoryStream();
#pragma warning disable IDE0079 // Remove unnecessary suppression
#pragma warning disable SYSLIB0011
            _formatter.Serialize(stream, original);
#pragma warning restore SYSLIB0011
            return stream.ToArray();
        }

        /// <inheritdoc />
        public override object Deserialize(byte[] serialized)
        {
            using var stream = new MemoryStream(serialized);
#pragma warning disable SYSLIB0011
            return _formatter.Deserialize(stream);
#pragma warning restore SYSLIB0011
#pragma warning restore IDE0079 // Remove unnecessary suppression
        }
    }
}
