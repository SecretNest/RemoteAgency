using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security;

namespace SecretNest.RemoteAgency.BinarySerializer
{
    /// <summary>
    /// Provides binary based serializing and deserializing methods for entities.
    /// </summary>
    /// <note type="warning">BinaryFormatter is included in this serializer, which is dangerous and should not be used. See <conceptualLink target="886b6555-5b60-46ed-b0e3-aa383c95108c" >Binary Formatter Warning</conceptualLink> for details.</note>
    /// <remarks><para>This class is not present in Neat release.</para></remarks>
    public class RemoteAgencyBinarySerializer: SerializingHelperBase<byte[], object>
    {
        readonly BinaryFormatter _formatter;

        /// <summary>
        /// Initializes an instance of RemoteAgencyBinarySerializer.
        /// </summary>
        /// <param name="securityIssueAcknowledged">This value needs to be set to <see langword="true" /> to confirm that the security issue is known, or an exception will be thrown. Default value is <see langword="false"/>.</param>
        /// <remarks><para>This constructor and this class are not present in Neat release.</para></remarks>
        public RemoteAgencyBinarySerializer(
            // ReSharper disable once ParameterOnlyUsedForPreconditionCheck.Local
            bool securityIssueAcknowledged = false)
        {
            if (!securityIssueAcknowledged)
                throw new SecurityException("BinaryFormatter is included in this serializer, which is dangerous and should not be used.");
            _formatter = new BinaryFormatter();
        }

        /// <summary>
        /// Serializes the entity object to binary format.
        /// </summary>
        /// <param name="original">The entity object to be serialized.</param>
        /// <returns>Serialized data.</returns>
        /// <remarks><para>This method and this class are not present in Neat release.</para></remarks>
        public override byte[] Serialize(object original)
        {
            using var stream = new MemoryStream();
#pragma warning disable IDE0079 // Remove unnecessary suppression
#pragma warning disable SYSLIB0011
            //ReSharper disable once CSharpWarnings::CS0618
            _formatter.Serialize(stream, original);
#pragma warning restore SYSLIB0011
            return stream.ToArray();
        }

        /// <summary>
        /// Deserializes the data to the original format from binary format.
        /// </summary>
        /// <param name="serialized">The serialized data to be deserialized.</param>
        /// <returns>Entity object.</returns>
        /// <remarks><para>This method and this class are not present in Neat release.</para></remarks>
        public override object Deserialize(byte[] serialized)
        {
            using var stream = new MemoryStream(serialized);
#pragma warning disable SYSLIB0011
            //ReSharper disable once CSharpWarnings::CS0618
            return _formatter.Deserialize(stream);
#pragma warning restore SYSLIB0011
#pragma warning restore IDE0079 // Remove unnecessary suppression
        }
    }
}
