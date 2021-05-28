using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Serialization;

namespace SecretNest.RemoteAgency.Inspecting
{
    /// <summary>
    /// The exception that is thrown when conflict of asset name is found.
    /// </summary>
    [Serializable]
#pragma warning disable CA1032 // Implement standard exception constructors
    public class AssetNameConflictException : InvalidAttributeDataException
#pragma warning restore CA1032 // Implement standard exception constructors
    {
        /// <summary>
        /// Initializes an instance of AssetNameConflictException.
        /// </summary>
        /// <param name="message">Exception message.</param>
        /// <param name="attribute">Attribute that cause this exception.</param>
        /// <param name="memberPath">Member path.</param>
        public AssetNameConflictException(string message, Attribute attribute, Stack<MemberInfo> memberPath) : base(message, attribute, memberPath)
        {
        }

        /// <summary>
        /// Initializes an instance of AssetNameConflictException.
        /// </summary>
        /// <param name="message">Exception message.</param>
        /// <param name="attribute">Attribute that cause this exception.</param>
        /// <param name="memberInfo">Member where this attribute is marked on.</param>
        /// <param name="memberParentPath">Parent path.</param>
        public AssetNameConflictException(string message, Attribute attribute, MemberInfo memberInfo, Stack<MemberInfo> memberParentPath) : base(message, attribute, memberInfo, memberParentPath)
        {
        }

        /// <summary>
        /// Initializes a new instance of the AssetNameConflictException class with serialized data.
        /// </summary>
        /// <param name="info">The SerializationInfo that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The StreamingContext that contains contextual information about the source or destination.</param>
        protected AssetNameConflictException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
