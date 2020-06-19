using System;
using System.Collections.Generic;
using System.Text;

namespace SecretNest.RemoteAgency
{
    public partial class RemoteAgency<TSerialized, TEntityBase>
    {
        /// <summary>
        /// Serializes an entity using the serializer passed from constructor.
        /// </summary>
        /// <param name="entity">Entity to serialize.</param>
        /// <returns>Serialized data.</returns>
        public TSerialized Serialize(TEntityBase entity)
        {
            return _serializingHelper.Serialize(entity);
        }

        /// <summary>
        /// Deserializes data using the serializer passed from constructor.
        /// </summary>
        /// <param name="serialized">Serialized data.</param>
        /// <returns>Entity object.</returns>
        public TEntityBase Deserialize(TSerialized serialized)
        {
            return _serializingHelper.Deserialize(serialized);
        }
    }
}
