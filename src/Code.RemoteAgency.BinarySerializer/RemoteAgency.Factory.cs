using System;
using System.Collections.Generic;
using System.Text;
using SecretNest.RemoteAgency.BinarySerializer;

namespace SecretNest.RemoteAgency
{
    public abstract partial class RemoteAgency
    {
        /// <summary>
        /// Creates an instance of Remote Agency using binary serializer.
        /// </summary>
        /// <returns>Created Remote Agency instance.</returns>
        public static RemoteAgency<byte[], object> CreateWithBinarySerializer()
        {
            return CreateWithoutCheck(new RemoteAgencyBinarySerializer(), new RemoteAgencyBinarySerializerEntityCodeBuilder());
        }
    }
}
