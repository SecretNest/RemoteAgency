using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace SecretNest.RemoteAgency
{
    /// <summary>
    /// The exception that is thrown when there is exception thrown from proxy or service wrapper type creating procedure.
    /// </summary>
    [Serializable]
    public sealed class TypeCreatingException : Exception
    {
        /// <summary>
        /// Exceptions.
        /// </summary>
        public TypeCreatingExceptionRecordCollection Records { get; }

        /// <summary>
        /// Initializes an instance of the TypeCreatingException.
        /// </summary>
        /// <param name="records">Exceptions.</param>
        public TypeCreatingException(List<TypeCreatingExceptionRecord> records)
        {
            Records = new TypeCreatingExceptionRecordCollection(records);
        }

        /// <summary>
        /// Initializes a new instance of the TypeCreatingException class with serialized data.
        /// </summary>
        /// <param name="info">The SerializationInfo that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The StreamingContext that contains contextual information about the source or destination.</param>
        private TypeCreatingException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            Records = (TypeCreatingExceptionRecordCollection)info.GetValue("Records", typeof(TypeCreatingExceptionRecordCollection));
        }

        /// <inheritdoc />
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("Records", Records);
        }
    }

    /// <summary>
    /// Contains messages from multiple exceptions thrown from type creating procedure.
    /// </summary>
    [Serializable]
    public sealed class TypeCreatingExceptionRecordCollection : IEnumerable<TypeCreatingExceptionRecord>, ISerializable
    {
        private List<TypeCreatingExceptionRecord> _records;

        /// <inheritdoc />
        public IEnumerator<TypeCreatingExceptionRecord> GetEnumerator()
        {
            return _records.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Initializes an instance of TypeCreatingExceptionRecordCollection.
        /// </summary>
        /// <param name="records">Exception records.</param>
        public TypeCreatingExceptionRecordCollection(List<TypeCreatingExceptionRecord> records)
        {
            _records = records;
        }

        /// <inheritdoc />
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("RecordCount", _records.Count);
            for (int i = 0; i < _records.Count; i++)
            {
                info.AddValue($"Record{i}", _records[i]);
            }
        }

        /// <summary>
        /// Initializes a new instance of the TypeCreatingExceptionRecordCollection class with serialized data.
        /// </summary>
        /// <param name="info">The SerializationInfo that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The StreamingContext that contains contextual information about the source or destination.</param>
        private TypeCreatingExceptionRecordCollection(SerializationInfo info, StreamingContext context)
        {
            var count = info.GetInt32("RecordCount");
            _records = new List<TypeCreatingExceptionRecord>(count);
            for (int i = 0; i < count; i++)
            {
                _records[i] =
                    (TypeCreatingExceptionRecord) info.GetValue($"Record{i}", typeof(TypeCreatingExceptionRecord));
            }
        }
    }

    /// <summary>
    /// Contains message from one exception thrown from type creating procedure.
    /// </summary>
    [Serializable]
    public sealed class TypeCreatingExceptionRecord : ISerializable
    {
        /// <summary>
        /// Exception Id.
        /// </summary>
        public string Id { get; }

        /// <summary>
        /// Exception Message.
        /// </summary>
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

        /// <summary>
        /// Initializes a new instance of the TypeCreatingExceptionRecord class with serialized data.
        /// </summary>
        /// <param name="info">The SerializationInfo that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The StreamingContext that contains contextual information about the source or destination.</param>
        private TypeCreatingExceptionRecord(SerializationInfo info, StreamingContext context)
        {
            Id = info.GetString("Id");
            Message = info?.GetString("Message");
        }

        /// <inheritdoc />
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Id", Id);
            info.AddValue("Message", Message);
        }
    }
}
