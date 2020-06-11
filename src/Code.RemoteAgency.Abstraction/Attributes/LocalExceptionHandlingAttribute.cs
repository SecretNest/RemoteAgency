using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace SecretNest.RemoteAgency.Attributes
{
    /// <summary>
    /// Specifies the local exception handling mode.
    /// </summary>
    /// <remarks>The attribute declared with interface affects the handling of serializing process when asset requested cannot be found, and has lower priority on all assets within the interface. The default setting is <see cref="LocalExceptionHandlingMode"/>.Suppress if this attribute absents.</remarks>
    /// <seealso cref="LocalExceptionHandlingMode"/>
    /// <seealso cref="ICommunicate{TSerialized}.RedirectedExceptionRaisedCallback"/>
    /// <seealso cref="RedirectedExceptionRaisedCallback"/>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Event | AttributeTargets.Interface, Inherited = true, AllowMultiple = false)]
    public class LocalExceptionHandlingAttribute : Attribute
    {
        /// <summary>
        /// Gets the setting of local exception handling mode.
        /// </summary>
        public LocalExceptionHandlingMode LocalExceptionHandlingMode { get; }

        /// <summary>
        /// Initializes an instance of the LocalExceptionHandlingAttribute.
        /// </summary>
        /// <param name="mode">Local exception handling mode.</param>
        /// <remarks>The default setting is <see cref="LocalExceptionHandlingMode"/>.Suppress if this attribute absents.</remarks>
        public LocalExceptionHandlingAttribute(LocalExceptionHandlingMode mode = LocalExceptionHandlingMode.Throw)
        {
            LocalExceptionHandlingMode = mode;
        }
    }

    /// <summary>
    /// Contains a list of local exception handling mode
    /// </summary>
    public enum LocalExceptionHandlingMode
    {
        /// <summary>
        /// Throw exceptions in place.
        /// </summary>
        Throw,
        /// <summary>
        /// Suppress exceptions.
        /// </summary>
        Suppress,
        /// <summary>
        /// Redirect exceptions to the handler specified.
        /// </summary>
        /// <seealso cref="ICommunicate{TSerialized}.RedirectedExceptionRaisedCallback"/>
        /// <seealso cref="RedirectedExceptionRaisedCallback"/>
        Redirect
    }
}
