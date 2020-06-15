using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using Newtonsoft.Json;

namespace SecretNest.RemoteAgency.BinarySerializer
{
    /// <summary>
    /// Provides binary based serializing and deserializing methods for entities.
    /// </summary>
    public class RemoteAgencyBinarySerializer: SerializingHelperBase<byte[], object>
    {
        readonly BinaryFormatter _formatter;

        /// <summary>
        /// Initializes an instance of RemoteAgencyBinarySerializer.
        /// </summary>
        public RemoteAgencyBinarySerializer()
        {
            _formatter = new BinaryFormatter();
        }

        /// <inheritdoc />
        public override byte[] Serialize(object original)
        {
            using (var stream = new MemoryStream())
            {
                _formatter.Serialize(stream, original);
                return stream.ToArray();
            }
        }

        /// <inheritdoc />
        public override object Deserialize(byte[] serialized)
        {
            using (var stream = new MemoryStream(serialized))
            {
                return _formatter.Deserialize(stream);
            }
        }
    }
}
