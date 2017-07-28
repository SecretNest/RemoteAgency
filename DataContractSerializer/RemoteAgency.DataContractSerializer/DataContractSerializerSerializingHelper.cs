using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;

namespace SecretNest.RemoteAgency
{
    /// <summary>
    /// Provides serializing and deserializing methods for entities and exceptions using DataContractSerializer."/>.
    /// </summary>
    /// <remarks>
    /// The name specified by <see cref="Attributes.CustomizedParameterEntityAttribute"/> or <see cref="Attributes.CustomizedReturnEntityAttribute"/> may be changed when generic type applied. Names of types will be added to the end of the name. e.g.: MyParameterEntityOfstringint
    /// </remarks>
    public class DataContractSerializerSerializingHelper : SerializingHelperBase<string, object>
    {
        /// <summary>
        /// Deserializes the data to the original format.
        /// </summary>
        /// <param name="serialized">The serialized data to be deserialized.</param>
        /// <param name="type">Type of the entity object.</param>
        /// <returns>Entity object.</returns>
        public override object Deserialize(string serialized, Type type)
        {
            if (string.IsNullOrEmpty(serialized)) return null;
            DataContractSerializer serializer = new DataContractSerializer(type);
            using (var stream = new MemoryStream())
            {
                using (var writer = new StreamWriter(stream))
                {
                    writer.Write(serialized);
                    writer.Flush();
                    stream.Seek(0, SeekOrigin.Begin);
                    return serializer.ReadObject(stream);
                }
            }
        }

        /// <summary>
        /// Deserializes the data to the exception object.
        /// </summary>
        /// <param name="serializedException">The serialized data to be deserialized.</param>
        /// <param name="exceptionType">The type of the inner exception</param>
        /// <returns>WrappedException object.</returns>
        public override WrappedException DeserializeException(string serializedException, Type exceptionType)
        {
            if (string.IsNullOrEmpty(serializedException)) return null;
            else return (WrappedException)Deserialize(serializedException, typeof(WrappedException<>).MakeGenericType(exceptionType));
        }

        /// <summary>
        /// Serializes the entity object.
        /// </summary>
        /// <param name="original">The entity object to be serialized.</param>
        /// <param name="type">Type of the entity object.</param>
        /// <returns>Serialized data.</returns>
        public override string Serialize(object original, Type type)
        {
            if (original == null) return null;
            DataContractSerializer serializer = new DataContractSerializer(type);
            using (var stream = new MemoryStream())
            {
                serializer.WriteObject(stream, original);
                stream.Seek(0, SeekOrigin.Begin);
                using (var reader = new StreamReader(stream))
                {
                    string result = reader.ReadToEnd();
                    return result.Replace(" xmlns:i=\"http://www.w3.org/2001/XMLSchema-instance\"", "");
                }
            }
        }

        /// <summary>
        /// Serializes the exception object.
        /// </summary>
        /// <param name="exception">The WrappedException object to be serialized.</param>
        /// <returns>Serialized data.</returns>
        public override string SerializeException(WrappedException exception)
        {
            if (exception == null) return null;
            return Serialize(exception, exception.GetType());
        }
    }
}
