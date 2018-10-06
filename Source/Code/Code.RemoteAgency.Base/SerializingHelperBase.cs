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
    public abstract class SerializingHelperBase<TSerialized, TEntityBase> where TEntityBase : class
    {
        /// <summary>
        /// Serializes the entity object.
        /// </summary>
        /// <param name="original">The entity object to be serialized.</param>
        /// <param name="type">Type of the entity object.</param>
        /// <returns>Serialized data.</returns>
        public abstract TSerialized Serialize(TEntityBase original, Type type);

        /// <summary>
        /// Serializes the entity object with exception tolerance.
        /// </summary>
        /// <param name="original">The entity object to be serialized.</param>
        /// <param name="type">Type of the entity object.</param>
        /// <param name="serializingException">The exception occurred in serializing process.</param>
        /// <returns>Serialized data.</returns>
        public virtual TSerialized SerializeWithExceptionTolerance(TEntityBase original, Type type, out Exception serializingException)
        {
            TSerialized result;
            try
            {
                result = Serialize(original, type);
                serializingException = null;
            }
            catch(Exception ex)
            {
                var exception = WrappedException.Create(ex);
                serializingException = ex;
                result = SerializeExceptionWithExceptionTolerance(exception, out _);
            }
            return result;
        }

        /// <summary>
        /// Deserializes the data to the original format.
        /// </summary>
        /// <param name="serialized">The serialized data to be deserialized.</param>
        /// <param name="type">Type of the entity object.</param>
        /// <returns>Entity object.</returns>
        public abstract TEntityBase Deserialize(TSerialized serialized, Type type);

        /// <summary>
        /// Deserializes the data to the original format with exception tolerance.
        /// </summary>
        /// <param name="serialized">The serialized data to be deserialized.</param>
        /// <param name="type">Type of the entity object.</param>
        /// <param name="wrappedException">The wrapped exception object which contains the exception occurred in deserializing process.</param>
        /// <param name="deserializingException">The exception occurred in deserializing process.</param>
        /// <returns>Entity object.</returns>
        public virtual TEntityBase DeserializeWithExceptionTolerance(TSerialized serialized, Type type, out WrappedException wrappedException, out Exception deserializingException)
        {
            TEntityBase result;
            try
            {
                result = Deserialize(serialized, type);
                wrappedException = null;
                deserializingException = null;
            }
            catch(Exception ex)
            {
                result = default(TEntityBase);
                deserializingException = ex;
                wrappedException = WrappedException.Create(ex);
            }
            return result;
        }

        /// <summary>
        /// Deserializes the data to the original format.
        /// </summary>
        /// <param name="serialized">The serialized data to be deserialized.</param>
        /// <param name="genericType">The generic type of the entity object.</param>
        /// <param name="genericArguments">The generic arguments of the <paramref name="genericType" >generic type</paramref> used in this entity object.</param>
        /// <returns>Entity object.</returns>
        public virtual TEntityBase Deserialize(TSerialized serialized, Type genericType, Type[] genericArguments)
        {
            return Deserialize(serialized, genericType.MakeGenericType(genericArguments));
        }

        /// <summary>
        /// Deserializes the data to the original format with exception tolerance.
        /// </summary>
        /// <param name="serialized">The serialized data to be deserialized.</param>
        /// <param name="genericType">The generic type of the entity object.</param>
        /// <param name="genericArguments">The generic arguments of the <paramref name="genericType" >generic type</paramref> used in this entity object.</param>
        /// <param name="wrappedException">The wrapped exception object which contains the exception occurred in deserializing process.</param>
        /// <param name="deserializingException">The exception occurred in deserializing process.</param>
        /// <returns>Entity object.</returns>
        public virtual TEntityBase DeserializeWithExceptionTolerance(TSerialized serialized, Type genericType, Type[] genericArguments, out WrappedException wrappedException, out Exception deserializingException)
        {
            return DeserializeWithExceptionTolerance(serialized, genericType.MakeGenericType(genericArguments), out wrappedException, out deserializingException);
        }

        /// <summary>
        /// Serializes the exception object.
        /// </summary>
        /// <param name="exception">The WrappedException object to be serialized.</param>
        /// <returns>Serialized data.</returns>
        /// <seealso cref="WrappedException"/>
        public abstract TSerialized SerializeException(WrappedException exception);

        /// <summary>
        /// Serializes the exception object with exception tolerance.
        /// </summary>
        /// <param name="exception">The WrappedException object to be serialized.</param>
        /// <param name="serializingException">The exception object occurred in serializing process.</param>
        /// <returns>Serialized data.</returns>
        /// <seealso cref="WrappedException"/>
        public virtual TSerialized SerializeExceptionWithExceptionTolerance(WrappedException exception, out Exception serializingException)
        {
            try
            {
                var result = SerializeException(exception);
                serializingException = null;
                return result;
            }
            catch (Exception ex)
            {
                serializingException = new Exception(string.Format("Exception thrown while serializing {0} object: {1}", exception.ExceptionType, ex.Message));
                return SerializeException(WrappedException.Create(serializingException));
            }
        }

        /// <summary>
        /// Deserializes the data to the exception object.
        /// </summary>
        /// <param name="serializedException">The serialized data to be deserialized.</param>
        /// <param name="exceptionType">The type of the inner exception</param>
        /// <returns>WrappedException object.</returns>
        /// <seealso cref="WrappedException"/>
        public abstract WrappedException DeserializeException(TSerialized serializedException, Type exceptionType);

        /// <summary>
        /// Deserializes the data to the exception object with exception tolerance.
        /// </summary>
        /// <param name="serializedException">The serialized data to be deserialized.</param>
        /// <param name="exceptionType">The type of the inner exception</param>
        /// <param name="deserializingException">The exception occurred in deserializing process.</param>
        /// <returns>WrappedException object.</returns>
        /// <seealso cref="WrappedException"/>
        public virtual WrappedException DeserializeExceptionWithExceptionTolerance(TSerialized serializedException, Type exceptionType, out Exception deserializingException)
        {
            try
            {
                WrappedException result = DeserializeException(serializedException, exceptionType);
                deserializingException = null;
                return result;
            }
            catch(Exception ex)
            {
                var normalized = new Exception(string.Format("Exception thrown while deserializing {0} object: {1}", exceptionType, ex.Message));
                deserializingException = ex;
                return WrappedException.Create(normalized);
            }
        }
    }
}
