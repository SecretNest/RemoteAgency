namespace SecretNest.RemoteAgency
{
    partial class RemoteAgencyBase
    {
        /// <summary>
        /// Enables or disables loopback address detection before sending a message. Default value is <see langword="true"/>.
        /// </summary>
        /// <remarks>When disabled, all internal messages, which sent from and to the same instance of Remote Agency, are treated as others.</remarks>
        public bool LoopbackAddressDetection { get; set; }
    }
}
