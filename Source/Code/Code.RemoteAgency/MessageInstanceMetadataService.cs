using System;
using System.Collections.Generic;
using System.Text;

namespace SecretNest.RemoteAgency
{
    /// <summary>
    /// Accesses the metadata of the current message instance.
    /// </summary>
    public static class MessageInstanceMetadataService
    {
        [ThreadStatic]
        internal static MessageInstanceMetadata messageInstanceMetadata;

        /// <summary>
        /// Gets the metadata of the current message instance.
        /// </summary>
        public static MessageInstanceMetadata CurrentMessageInstanceMetadata => messageInstanceMetadata;
    }
}
