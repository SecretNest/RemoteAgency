using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;

namespace SecretNest.RemoteAgency.Inspecting
{
    /// <summary>
    /// The exception that is thrown when conflict of property name in entity is found.
    /// </summary>
    [Serializable]
    public class EntityPropertyNameConflictException : InvalidAttributeDataException
    { 
        /// <summary>
        /// Gets the parameter which the attribute is on.
        /// </summary>
        /// <remarks>Only valid when <see cref="IsCausedByReturnValue"/> is set to <see langword="false"/>.</remarks>
        public ParameterInfo Parameter { get; }

        /// <summary>
        /// Gets whether this exception is caused by the attribute marked about the return value.
        /// </summary>
        public bool IsCausedByReturnValue { get; }

        /// <summary>
        /// Initializes an instance of EntityPropertyNameConflictException.
        /// </summary>
        /// <param name="message">Exception message.</param>
        /// <param name="attribute">Attribute that cause this exception.</param>
        /// <param name="parameter">Parameter which the attribute is on.</param>
        /// <param name="memberPath">Member path.</param>
        public EntityPropertyNameConflictException(string message, Attribute attribute, ParameterInfo parameter, Stack<MemberInfo> memberPath) : base(message, attribute, memberPath)
        {
            Parameter = parameter;
        }

        /// <summary>
        /// Initializes an instance of EntityPropertyNameConflictException.
        /// </summary>
        /// <param name="message">Exception message.</param>
        /// <param name="attribute">Attribute that cause this exception.</param>
        /// <param name="parameter">Parameter which the attribute is on.</param>
        /// <param name="memberInfo">Member where this attribute is marked on.</param>
        /// <param name="memberParentPath">Parent path.</param>
        public EntityPropertyNameConflictException(string message, Attribute attribute, ParameterInfo parameter, MemberInfo memberInfo, Stack<MemberInfo> memberParentPath) : base(message, attribute, memberInfo, memberParentPath)
        {
            Parameter = parameter;
        }

        /// <summary>
        /// Initializes an instance of EntityPropertyNameConflictException.
        /// </summary>
        /// <param name="message">Exception message.</param>
        /// <param name="attribute">Attribute that cause this exception.</param>
        /// <param name="memberPath">Member path.</param>
        public EntityPropertyNameConflictException(string message, Attribute attribute, Stack<MemberInfo> memberPath) : base(message, attribute, memberPath)
        {
            IsCausedByReturnValue = true;
        }

        /// <summary>
        /// Initializes an instance of EntityPropertyNameConflictException.
        /// </summary>
        /// <param name="message">Exception message.</param>
        /// <param name="attribute">Attribute that cause this exception.</param>
        /// <param name="memberInfo">Member where this attribute is marked on.</param>
        /// <param name="memberParentPath">Parent path.</param>
        public EntityPropertyNameConflictException(string message, Attribute attribute, MemberInfo memberInfo, Stack<MemberInfo> memberParentPath) : base(message, attribute, memberInfo, memberParentPath)
        {
            IsCausedByReturnValue = true;
        }

        /// <summary>
        /// Initializes a new instance of the EntityPropertyNameConflictException class with serialized data.
        /// </summary>
        /// <param name="info">The SerializationInfo that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The StreamingContext that contains contextual information about the source or destination.</param>
        protected EntityPropertyNameConflictException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            Parameter = (ParameterInfo) info.GetValue("Parameter", typeof(ParameterInfo));
            IsCausedByReturnValue = info.GetBoolean("IsCausedByReturnValue");
        }
        
        /// <inheritdoc />
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("Parameter", Parameter);
            info.AddValue("IsCausedByReturnValue", IsCausedByReturnValue);
        }

    }
}