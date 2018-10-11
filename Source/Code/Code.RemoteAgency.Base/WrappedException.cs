using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using System.Xml.Serialization;

namespace SecretNest.RemoteAgency
{
    /// <summary>
    /// Represents a remote site exception. This is an abstract class.
    /// </summary>
    /// <seealso cref="WrappedException{T}"/>
    [DataContract(Namespace = "")]
    [Serializable()]
    public abstract class WrappedException
    {
        /// <summary>
        /// Gets the exception from the remote site
        /// </summary>
        [IgnoreDataMember][XmlIgnore] public abstract Exception ExceptionGeneric { get; internal set; }

        /// <summary>
        /// Gets the type of the exception
        /// </summary>
        [IgnoreDataMember][XmlIgnore] public abstract Type ExceptionType { get; }

        /// <summary>
        /// Creates and returns a string representation of the inner exception.
        /// </summary>
        /// <returns>A string representation of the inner exception.</returns>
        public abstract override string ToString();

        /// <summary>
        /// Creates a new instance of the WrappedException class.
        /// </summary>
        /// <param name="exception">Exception object.</param>
        /// <returns>Instance representing a remote site exception.</returns>
        public static WrappedException Create(Exception exception)
        {
            var exceptionType = exception.GetType();
            var wrappedType = typeof(WrappedException<>).MakeGenericType(exceptionType);
            var result = (WrappedException)FastActivator.CreateInstance(wrappedType);
            result.ExceptionGeneric = exception;

            return result;
        }
    }

    /// <summary>
    /// Represents a remote site exception.
    /// </summary>
    /// <typeparam name="T">Type of the inner exception.</typeparam>
    /// <seealso cref="WrappedException"/>
    [DataContract(Namespace = "")]
    [Serializable()]
    public class WrappedException<T> : WrappedException where T : Exception
    {
        //[DataMember]
        //T InnerException { get; set; }

        /// <summary>
        /// Initializes a new instance of WrappedException class.
        /// </summary>
        /// <param name="exception">Exception object</param>
        /// <returns></returns>
        public WrappedException(T exception)
        {
            Exception = exception;
        }

        /// <summary>
        /// Initializes a new instance of WrappedException class.
        /// </summary>
        public WrappedException() { }

        /// <summary>
        /// Gets the exception from the remote site
        /// </summary>
        [IgnoreDataMember]
        [XmlIgnore]
        public override Exception ExceptionGeneric { get { return Exception; } internal set { Exception = (T)value; } }

        /// <summary>
        /// Gets the exception from the remote site
        /// </summary>
        [DataMember]
        public T Exception { get; internal set; }

        /// <summary>
        /// Gets the type of the exception
        /// </summary>
        [IgnoreDataMember]
        [XmlIgnore]
        public override Type ExceptionType { get { return typeof(T); } }

        /// <summary>
        /// Creates and returns a string representation of the inner exception.
        /// </summary>
        /// <returns>A string representation of the inner exception.</returns>
        public override string ToString() => Exception.ToString();
    }
}
