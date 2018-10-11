using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace SecretNest.RemoteAgency
{
    /// <summary>
    /// The exception that is thrown when the target site cannot be found in target site table and default site is not set either.
    /// </summary>
    /// <seealso cref="RemoteAgencyManager{TNetworkMessage, TSerialized, TEntityBase}.AddTargetSite(Guid, Guid)"/>
    /// <seealso cref="RemoteAgencyManager{TNetworkMessage, TSerialized, TEntityBase}.RemoveTargetSite(Guid)"/>
    /// <seealso cref="RemoteAgencyManager{TNetworkMessage, TSerialized, TEntityBase}.ResetTargetSite"/>
    /// <seealso cref="RemoteAgencyManager{TNetworkMessage, TSerialized, TEntityBase}.DefaultTargetSiteId"/>
    [Serializable]
    public class TargetSiteNotFoundException : NullReferenceException
    {
        /// <summary>
        /// Gets the instance id or Message id based on the value of <see cref="IdType"/>.
        /// </summary>
        public Guid Id { get; }
        /// <summary>
        /// Gets the id type.
        /// </summary>
        public TargetSiteNotFoundIdType IdType { get; }

        /// <summary>
        /// Initializes an instance of TargetSiteNotFoundException.
        /// </summary>
        /// <param name="idType">Id type.</param>
        /// <param name="id">Instance id or Message id based on the value of <paramref name="idType"/>.</param>
        internal TargetSiteNotFoundException(TargetSiteNotFoundIdType idType, Guid id)
        {
            IdType = idType;
            Id = id;
        }

        /// <summary>
        /// Initializes a new instance of the TargetSiteNotFoundException class with serialized data.
        /// </summary>
        /// <param name="info">The SerializationInfo that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The StreamingContext that contains contextual information about the source or destination.</param>
        public TargetSiteNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        { }

        /// <summary>
        /// Gets the error message of the current exception.
        /// </summary>
        public override string Message => string.Format("The site for {0} {1} not found.", IdType, Id);

        /// <summary>
        /// Creates and returns a string representation of the current exception.
        /// </summary>
        /// <returns>A string representation of the current exception.</returns>
        public override string ToString() => Message;
    }

    /// <summary>
    /// Contains a list of id types for <see cref="TargetSiteNotFoundException"/>.
    /// </summary>
    /// <seealso cref="TargetSiteNotFoundException"/>
    [DataContract(Namespace = "")]
    [Serializable()]
    public enum TargetSiteNotFoundIdType
    {
        /// <summary>
        /// Instance id.
        /// </summary>
        [EnumMember]
        InstanceId,
        /// <summary>
        /// Message id.
        /// </summary>
        [EnumMember]
        MessageId
    }
}
