using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Serialization;

namespace SecretNest.RemoteAgency.Inspecting
{
    /// <summary>
    /// The exception that is thrown when conflict of property name in entity is found.
    /// </summary>
    [Serializable]
#pragma warning disable CA1032 // Implement standard exception constructors
    public class EntityPropertyNameConflictException : InvalidAttributeDataException
#pragma warning restore CA1032 // Implement standard exception constructors
    { 
        /// <summary>
        /// Gets the parameter which the attribute is on.
        /// </summary>
        /// <remarks>Only valid when <see cref="CausedMemberType"/> is set to <see cref="EntityPropertyNameConflictExceptionCausedMemberType"/>.Parameter.</remarks>
        public ParameterInfo Parameter { get; }

        /// <summary>
        /// Gets the member type where this exception is caused.
        /// </summary>
        public EntityPropertyNameConflictExceptionCausedMemberType CausedMemberType { get; }

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
            CausedMemberType = EntityPropertyNameConflictExceptionCausedMemberType.Parameter;
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
            CausedMemberType = EntityPropertyNameConflictExceptionCausedMemberType.Parameter;
        }

        /// <summary>
        /// Initializes an instance of EntityPropertyNameConflictException.
        /// </summary>
        /// <param name="message">Exception message.</param>
        /// <param name="attribute">Attribute that cause this exception.</param>
        /// <param name="causedMemberType">Member type where this exception is caused.</param>
        /// <param name="memberPath">Member path.</param>
        public EntityPropertyNameConflictException(string message, Attribute attribute, EntityPropertyNameConflictExceptionCausedMemberType causedMemberType, Stack<MemberInfo> memberPath) : base(message, attribute, memberPath)
        {
            CausedMemberType = causedMemberType;
        }

        /// <summary>
        /// Initializes an instance of EntityPropertyNameConflictException.
        /// </summary>
        /// <param name="message">Exception message.</param>
        /// <param name="attribute">Attribute that cause this exception.</param>
        /// <param name="memberInfo">Member where this attribute is marked on.</param>
        /// <param name="causedMemberType">Member type where this exception is caused.</param>
        /// <param name="memberParentPath">Parent path.</param>
        public EntityPropertyNameConflictException(string message, Attribute attribute, MemberInfo memberInfo, EntityPropertyNameConflictExceptionCausedMemberType causedMemberType, Stack<MemberInfo> memberParentPath) : base(message, attribute, memberInfo, memberParentPath)
        {
            CausedMemberType = causedMemberType;
        }

        /// <summary>
        /// Initializes a new instance of the EntityPropertyNameConflictException class with serialized data.
        /// </summary>
        /// <param name="info">The SerializationInfo that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The StreamingContext that contains contextual information about the source or destination.</param>
        protected EntityPropertyNameConflictException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            Parameter = (ParameterInfo) info.GetValue("Parameter", typeof(ParameterInfo));
            CausedMemberType = (EntityPropertyNameConflictExceptionCausedMemberType) Enum.Parse(
                typeof(EntityPropertyNameConflictExceptionCausedMemberType), info.GetString("CausedMemberType")!);
        }
        
        /// <inheritdoc />
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("Parameter", Parameter);
            info.AddValue("CausedMemberType", CausedMemberType.ToString());
        }

    }

    /// <summary>
    /// Contains a list of the type of member which can cause <see cref="EntityPropertyNameConflictException"/>.
    /// </summary>
    public enum EntityPropertyNameConflictExceptionCausedMemberType
    {
        /// <summary>
        /// Parameter.
        /// </summary>
        /// <remarks>The parameter object should be set to <see cref="EntityPropertyNameConflictException.Parameter"/>.</remarks>
        Parameter,
        /// <summary>
        /// Return value.
        /// </summary>
        ReturnValue,
        /// <summary>
        /// Property
        /// </summary>
        Property
    }
}