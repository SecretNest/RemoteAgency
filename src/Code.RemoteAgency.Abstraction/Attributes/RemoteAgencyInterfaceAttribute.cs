using System;

namespace SecretNest.RemoteAgency.Attributes
{
    /// <summary>
    /// Indicates this interface should be processed by Remote Agency.
    /// </summary>
    /// <remarks>
    /// This attribute has no use in this release. Future releases may preprocess the Interface that marks this attribute to enable acceleration.
    /// </remarks>
    [AttributeUsage(AttributeTargets.Interface, Inherited = true, AllowMultiple = true)]
    public class RemoteAgencyInterfaceAttribute : Attribute
    {
    }
}
