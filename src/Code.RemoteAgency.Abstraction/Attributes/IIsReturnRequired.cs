namespace SecretNest.RemoteAgency.Attributes
{
    /// <summary>
    /// Specifies the class contains the properties to describe a return required element.
    /// </summary>
    public interface IIsReturnRequired
    {
        /// <summary>
        /// Gets whether the element should be included in return entity.
        /// </summary>
        bool IsIncludedInReturning { get; }

        /// <summary>
        /// Gets the preferred property name in response entity.
        /// </summary>
        /// <remarks>When the value is <see langword="null"/> or empty string, name is chosen automatically.</remarks>
        string ResponseEntityPropertyName { get; }

        /// <summary>
        /// Gets whether the element should be included in return entity when exception thrown by the user code on the remote site.
        /// </summary>
        bool IsIncludedWhenExceptionThrown { get; }

    }
}
