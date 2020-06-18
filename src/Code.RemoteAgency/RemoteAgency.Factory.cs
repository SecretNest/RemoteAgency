using System;
using System.Collections.Generic;
using System.Text;

namespace SecretNest.RemoteAgency
{
    public abstract partial class RemoteAgency
    {
        /// <summary>
        /// Creates an instance of Remote Agency.
        /// </summary>
        /// <typeparam name="TSerialized">Type of the serialized data.</typeparam>
        /// <typeparam name="TEntityBase">Type of the parent class of all entities.</typeparam>
        /// <param name="serializingHelper">Serializer helper.</param>
        /// <param name="entityCodeBuilder">Entity code builder.</param>
        /// <returns>Created Remote Agency instance.</returns>
        public static RemoteAgency<TSerialized, TEntityBase> Create<TSerialized, TEntityBase>(
            SerializingHelperBase<TSerialized, TEntityBase> serializingHelper, EntityCodeBuilderBase entityCodeBuilder)
        {
            if (serializingHelper == null)
                throw new ArgumentNullException(nameof(serializingHelper));
            if (entityCodeBuilder == null)
                throw new ArgumentNullException(nameof(entityCodeBuilder));

            return CreateWithoutCheck(serializingHelper, entityCodeBuilder);
        }

        static RemoteAgency<TSerialized, TEntityBase> CreateWithoutCheck<TSerialized, TEntityBase>(
            SerializingHelperBase<TSerialized, TEntityBase> serializingHelper, EntityCodeBuilderBase entityCodeBuilder)
        {
            return new RemoteAgency<TSerialized, TEntityBase>(serializingHelper, entityCodeBuilder);
        }
    }
}
