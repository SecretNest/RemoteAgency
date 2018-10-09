using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace SecretNest.RemoteAgency
{
    /// <summary>
    /// The exception that is thrown when there is exception thrown from proxy or service wrapper type creating procedure.
    /// </summary>
    [DataContract(Namespace = "")]
    public class TypeCreatingException : Exception
    {
        /// <summary>
        /// Collection of exceptions.
        /// </summary>
        [DataMember]
        public IReadOnlyCollection<TypeCreatingExceptionRecord> Records { get; }

        /// <summary>
        /// Initializes an instance of the TypeCreatingException.
        /// </summary>
        /// <param name="records">Collection of exceptions.</param>
        internal TypeCreatingException(IReadOnlyCollection<TypeCreatingExceptionRecord> records)
        {
            Records = records;
        }
    }

    /// <summary>
    /// Contains one exception thrown from type creating procedure.
    /// </summary>
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
        internal TypeCreatingExceptionRecord(string id, string message)
        {
            Id = id;
            Message = message;
        }
    }
}
