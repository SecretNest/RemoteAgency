using System.Collections.Generic;
using System.Reflection;
using Newtonsoft.Json;

namespace SecretNest.RemoteAgency.JsonSerializer
{
    /// <summary>
    /// Provides json based serializing and deserializing methods for entities.
    /// </summary>
    /// <remarks><para>This class is not present in Neat release.</para></remarks>
    public class RemoteAgencyJsonSerializer : SerializingHelperBase<string, object>
    {
        /// <summary>
        /// Initializes an instance of RemoteAgencyJsonSerializer.
        /// </summary>
        /// <param name="intented">Whether should cause child objects to be indented according to the Indentation and IndentChar settings. Default value is false.</param>
        /// <param name="includingFullAssemblyName">Whether should include full assembly name in serialized data and use <code>Load</code> method of the <see cref="Assembly"/> class is used to load the assembly instead of using <code>Load</code> method. Default value is true</param>
        /// <remarks><para>This constructor and this class are not present in Neat release.</para></remarks>
        public RemoteAgencyJsonSerializer(bool intented = false, bool includingFullAssemblyName = true)
        {
            _setting = new JsonSerializerSettings()
            {
                TypeNameAssemblyFormatHandling =
                    includingFullAssemblyName
                        ? TypeNameAssemblyFormatHandling.Full
                        : TypeNameAssemblyFormatHandling.Simple,
                TypeNameHandling = TypeNameHandling.Auto,
                Formatting = intented ? Formatting.Indented : Formatting.None,
                Converters = new List<JsonConverter>() {new Newtonsoft.Json.Converters.StringEnumConverter()}
            };
        }

        private readonly JsonSerializerSettings _setting;

        /// <summary>
        /// Serializes the entity object to json format.
        /// </summary>
        /// <param name="original">The entity object to be serialized.</param>
        /// <returns>Serialized data.</returns>
        /// <remarks><para>This method and this class are not present in Neat release.</para></remarks>
        public override string Serialize(object original)
        {
            //typeof(object) is placed here to force the root type is included.
            return JsonConvert.SerializeObject(original, typeof(object), _setting);
        }

        /// <summary>
        /// Deserializes the data to the original format from json format.
        /// </summary>
        /// <param name="serialized">The serialized data to be deserialized.</param>
        /// <returns>Entity object.</returns>
        /// <remarks><para>This method and this class are not present in Neat release.</para></remarks>
        public override object Deserialize(string serialized)
        {
            return JsonConvert.DeserializeObject(serialized, _setting);
        }
    }
}
