namespace SecretNest.RemoteAgency.Attributes
{
    /// <summary>
    /// Specifies the class contains the property to describe whether the operating is one-way.
    /// </summary>
    public interface IIsOneWay
    {
        /// <summary>
        /// Gets whether the operating is one-way.
        /// </summary>
        bool IsOneWay { get; }
    }
}
