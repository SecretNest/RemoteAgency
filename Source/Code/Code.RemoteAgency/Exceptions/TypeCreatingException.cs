using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace SecretNest.RemoteAgency
{
    /// <summary>
    /// The exception that is thrown when there is exception thrown from proxy or service wrapper type creating procedure.
    /// </summary>
    [Serializable]
    public class TypeCreatingException : Exception
    {
        /// <summary>
        /// Collection of exceptions.
        /// </summary>
        public IReadOnlyCollection<TypeCreatingExceptionRecord> Records { get; }

        /// <summary>
        /// Initializes an instance of the TypeCreatingException.
        /// </summary>
        /// <param name="records">Collection of exceptions.</param>
        internal TypeCreatingException(IReadOnlyCollection<TypeCreatingExceptionRecord> records)
        {
            Records = records;
        }

        /// <summary>
        /// Initializes a new instance of the TypeCreatingException class with serialized data.
        /// </summary>
        /// <param name="info">The SerializationInfo that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The StreamingContext that contains contextual information about the source or destination.</param>
        public TypeCreatingException(SerializationInfo info, StreamingContext context) : base(info, context)
        { }
    }

    /// <summary>
    /// Contains one exception thrown from type creating procedure.
    /// </summary>
    [Serializable]
    [DataContract(Namespace = "")]
    public class TypeCreatingExceptionRecord
    {
        /// <summary>
        /// Exception Id.
        /// </summary>
        [DataMember]
        public string Id { get; }

        /// <summary>
        /// Exception Message.
        /// </summary>
        [DataMember]
        public string Message { get; }

        /// <summary>
        /// Initializes an instance of the TypeCreatingExceptionRecord.
        /// </summary>
        /// <param name="id">Exception Id.</param>
        /// <param name="message">Exception Message.</param>
        public TypeCreatingExceptionRecord(string id, string message)
        {
            Id = id;
            Message = message;
        }

        /// <summary>
        /// Initializes an instance of the TypeCreatingExceptionRecord. This method is kept for serializing supporting.
        /// </summary>
        public TypeCreatingExceptionRecord()
        { }
    }
}
