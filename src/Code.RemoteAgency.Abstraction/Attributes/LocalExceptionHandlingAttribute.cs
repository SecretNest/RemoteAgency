using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace SecretNest.RemoteAgency.Attributes
{
    /// <summary>
    /// Specifies the local exception handling mode.
    /// </summary>
    /// <remarks>The attribute declared with interface has lower priority on all assets within the interface. The default setting is <see cref="LocalExceptionHandlingMode"/>.Redirect if this attribute is absent.</remarks>
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
    /// Contains a list of local exception handling mode.
    /// </summary>
    public enum LocalExceptionHandlingMode
    {
        /// <summary>
        /// Throws exceptions in place.
        /// </summary>
        Throw,
        /// <summary>
        /// Suppresses exceptions.
        /// </summary>
        Suppress,
        /// <summary>
        /// Redirects exceptions to the handler specified. Throws when handler is absent.
        /// </summary>
        Redirect
    }
}
