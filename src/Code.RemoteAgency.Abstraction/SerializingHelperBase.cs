using System;
using System.Collections.Generic;
using System.Text;

namespace SecretNest.RemoteAgency
{
    /// <summary>
    /// Provides serializing and deserializing methods for entities and exceptions. This is an abstract class.
    /// </summary>
    /// <typeparam name="TSerialized">Type of the serialized data.</typeparam>
    /// <typeparam name="TEntityBase">Type of the parent class of all entities.</typeparam>
    public abstract class SerializingHelperBase<TSerialized, TEntityBase>
        where TEntityBase : class, IRemoteAgencyMessage<TSerialized>, new()
    {
        /// <summary>
        /// Serializes the entity object.
        /// </summary>
        /// <param name="original">The entity object to be serialized.</param>
        /// <returns>Serialized data.</returns>
        public abstract TSerialized Serialize(TEntityBase original);

        /// <summary>
        /// Serializes the entity object with exception tolerance.
        /// </summary>
        /// <param name="original">The entity object to be serialized.</param>
        /// <param name="serializingException">The exception occurred in serializing process.</param>
        /// <returns>Serialized data.</returns>
        public virtual TSerialized SerializeWithExceptionTolerance(TEntityBase original, out Exception serializingException)
        {
            TSerialized result;
            try
            {
                result = Serialize(original);
                serializingException = null;
            }
            catch (Exception ex)
            {
                serializingException = ex;

                TEntityBase exceptionPackage = new TEntityBase()
                {
                    AssetName = original.AssetName,
                    IsOneWay = true,
                    Exception = ex,
                    MessageId = original.MessageId,
                    MessageType = original.MessageType,
                    SenderInstanceId = original.SenderInstanceId,
                    SenderSiteId = original.SenderSiteId,
                    TargetInstanceId = original.TargetInstanceId,
                    TargetSiteId = original.TargetSiteId
                };

                result = Serialize(exceptionPackage);
            }

            return result;
        }

        /// <summary>
        /// Deserializes the data to the original format.
        /// </summary>
        /// <param name="serialized">The serialized data to be deserialized.</param>
        /// <returns>Entity object.</returns>
        public abstract TEntityBase Deserialize(TSerialized serialized);

        /// <summary>
        /// Deserializes the data to the original format with exception tolerance.
        /// </summary>
        /// <param name="serialized">The serialized data to be deserialized.</param>
        /// <param name="deserializingException">The exception occurred in deserializing process.</param>
        /// <returns>Entity object.</returns>
        public virtual TEntityBase DeserializeWithExceptionTolerance(TSerialized serialized, out Exception deserializingException)
        {
            TEntityBase result;
            try
            {
                result = Deserialize(serialized);
                deserializingException = null;
            }
            catch (Exception ex)
            {
                result = default;
                deserializingException = ex;
            }

            return result;
        }
    }
}
