using System;

namespace SecretNest.RemoteAgency.Attributes
{
    /// <summary>
    /// Specifies the local exception handling mode.
    /// </summary>
    /// <remarks>
    /// <para>This attribute affects the code being called remotely: method in service wrapper, event adding and removing in service wrapper, property getting and setting in service wrapper and event raising in proxy.</para>
    /// <para>The attribute declared with interface has lower priority on all assets within the interface. The default setting is <see cref="LocalExceptionHandlingMode"/>.Redirect if this attribute is absent.</para>
    /// <para>The one marked on the event has higher priority than the one marked on the delegate of the same event.</para>
    /// <para>The default setting is the same as interface when this attribute absents on asset. The default setting is <see cref="LocalExceptionHandlingMode"/>.Suppress when this attribute absents on interface.</para>
    /// </remarks>
    /// <conceptualLink target="14c3caef-7392-4f68-b7eb-d0bb014a2e4c#InterfaceLevel" />
    /// <conceptualLink target="5e4cbd69-ed6d-4b66-8496-4925ae64b4c7" />
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Event | AttributeTargets.Interface | AttributeTargets.Delegate, Inherited = true, AllowMultiple = false)]
    public sealed class LocalExceptionHandlingAttribute : Attribute
    {
        /// <summary>
        /// Gets the setting of local exception handling mode.
        /// </summary>
        public LocalExceptionHandlingMode LocalExceptionHandlingMode { get; }

        /// <summary>
        /// Initializes an instance of the LocalExceptionHandlingAttribute.
        /// </summary>
        /// <param name="mode">Local exception handling mode. Default value is <see cref="LocalExceptionHandlingMode"/>.Redirect.</param>
        public LocalExceptionHandlingAttribute(LocalExceptionHandlingMode mode = LocalExceptionHandlingMode.Redirect)
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
