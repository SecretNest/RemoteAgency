using System;
using System.Collections.Generic;
using System.Text;
using SecretNest.RemoteAgency.JsonSerializer;

namespace SecretNest.RemoteAgency
{
    public abstract partial class RemoteAgency
    {
        /// <summary>
        /// Creates an instance of Remote Agency using Json serializer.
        /// </summary>
        /// <returns>Created Remote Agency instance.</returns>
        public static RemoteAgency<string, object> CreateWithJsonSerializer()
        {
            return CreateWithoutCheck(new RemoteAgencyJsonSerializer(), new RemoteAgencyJsonSerializerEntityCodeBuilder());
        }
    }
}
