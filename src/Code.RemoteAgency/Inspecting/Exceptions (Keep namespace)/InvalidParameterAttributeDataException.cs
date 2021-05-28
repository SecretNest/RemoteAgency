using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Serialization;

namespace SecretNest.RemoteAgency.Inspecting
{
    /// <summary>
    /// The exception that is thrown when the invalid attribute or data within attribute is found on a parameter.
    /// </summary>
    [Serializable]
#pragma warning disable CA1032 // Implement standard exception constructors
    public class InvalidParameterAttributeDataException : InvalidAttributeDataException
#pragma warning restore CA1032 // Implement standard exception constructors
    {
        /// <summary>
        /// Gets the parameter which the attribute is on.
        /// </summary>
        public ParameterInfo Parameter { get; }

        /// <summary>
        /// Initializes an instance of InvalidParameterAttributeDataException.
        /// </summary>
        /// <param name="message">Exception message.</param>
        /// <param name="attribute">Attribute that cause this exception.</param>
        /// <param name="parameter">Parameter which the attribute is on.</param>
        /// <param name="memberPath">Member path.</param>
        public InvalidParameterAttributeDataException(string message, Attribute attribute, ParameterInfo parameter, Stack<MemberInfo> memberPath) : base(message, attribute, memberPath)
        {
            Parameter = parameter;
        }

        /// <summary>
        /// Initializes an instance of InvalidParameterAttributeDataException.
        /// </summary>
        /// <param name="message">Exception message.</param>
        /// <param name="attribute">Attribute that cause this exception.</param>
        /// <param name="parameter">Parameter which the attribute is on.</param>
        /// <param name="memberInfo">Member where this attribute is marked on.</param>
        /// <param name="memberParentPath">Parent path.</param>
        public InvalidParameterAttributeDataException(string message, Attribute attribute, ParameterInfo parameter, MemberInfo memberInfo, Stack<MemberInfo> memberParentPath) : base(message, attribute, memberInfo, memberParentPath)
        {
            Parameter = parameter;
        }

        /// <summary>
        /// Initializes a new instance of the InvalidParameterAttributeDataException class with serialized data.
        /// </summary>
        /// <param name="info">The SerializationInfo that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The StreamingContext that contains contextual information about the source or destination.</param>
        protected InvalidParameterAttributeDataException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            Parameter = (ParameterInfo) info.GetValue("Parameter", typeof(ParameterInfo));
        }

        /// <inheritdoc />
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("Parameter", Parameter);
        }
    }
}
