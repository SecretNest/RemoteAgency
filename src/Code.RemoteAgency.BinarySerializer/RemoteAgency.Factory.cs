﻿using System;
using System.Collections.Generic;
using System.Text;
using SecretNest.RemoteAgency.BinarySerializer;

namespace SecretNest.RemoteAgency
{
    abstract partial class RemoteAgency
    {
        /// <summary>
        /// Creates an instance of Remote Agency using binary serializer.
        /// </summary>
        /// <param name="siteId">Site id. A randomized value is used when it is set as <see cref="Guid"/>.Empty or absent.</param>
        /// <returns>Created Remote Agency instance.</returns>
        public static RemoteAgency<byte[], object> CreateWithBinarySerializer(Guid? siteId = null)
        {
            return CreateWithoutCheck(new RemoteAgencyBinarySerializer(), new RemoteAgencyBinarySerializerEntityCodeBuilder(), siteId);
        }
    }
}
