using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Newtonsoft.Json;

namespace SecretNest.RemoteAgency.JsonSerializer
{
    /// <summary>
    /// Provides json based serializing and deserializing methods for entities.
    /// </summary>
    public class RemoteAgencyJsonSerializer : SerializingHelperBase<string, object>
    {
        private readonly Formatting _formatting;

        /// <summary>
        /// Initializes an instance of RemoteAgencyJsonSerializer.
        /// </summary>
        /// <param name="intented">Whether should cause child objects to be indented according to the Indentation and IndentChar settings. Default value is false.</param>
        /// <param name="includingFullAssemblyName">Whether should include full assembly name in serialized data and use <code>Load</code> method of the <see cref="Assembly"/> class is used to load the assembly instead of using <code>Load</code> method. Default value is true</param>
        public RemoteAgencyJsonSerializer(bool intented = false, bool includingFullAssemblyName = true)
        {
            _formatting = intented ? Formatting.Indented : Formatting.None;
            _setting = new JsonSerializerSettings()
            {
                TypeNameAssemblyFormatHandling = includingFullAssemblyName ? TypeNameAssemblyFormatHandling.Full : TypeNameAssemblyFormatHandling.Simple,
                TypeNameHandling = TypeNameHandling.Auto,
                Converters = new List<JsonConverter>() {new Newtonsoft.Json.Converters.StringEnumConverter()}
            };
        }

        private readonly JsonSerializerSettings _setting;

        /// <inheritdoc />
        public override string Serialize(object original)
        {
            return JsonConvert.SerializeObject(original, _formatting, _setting);
        }

        /// <inheritdoc />
        public override object Deserialize(string serialized)
        {
            return JsonConvert.DeserializeObject(serialized, _setting);
        }
    }
}
