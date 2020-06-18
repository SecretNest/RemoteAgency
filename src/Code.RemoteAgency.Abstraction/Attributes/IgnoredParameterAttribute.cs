using System;
using System.Collections.Generic;
using System.Text;

namespace SecretNest.RemoteAgency.Attributes
{
    /// <summary>
    /// Specifies the parameter should or should not be transferred to remote site. If this attribute absent, the default behavior is transferring all parameters.
    /// </summary>
    /// <remarks>When <see cref="IgnoredInParameter"/> or <see cref="IgnoredInReturn"/> is set to true, the <see cref="ParameterTwoWayPropertyAttribute"/> and <see cref="EventParameterTwoWayPropertyAttribute"/> will be ignored.</remarks>
    /// <seealso cref="EventParameterIgnoredAttribute"/>
    /// <seealso cref="ParameterTwoWayPropertyAttribute"/>
    /// <seealso cref="EventParameterTwoWayPropertyAttribute"/>
    [AttributeUsage(AttributeTargets.Parameter, Inherited = true, AllowMultiple = false)]
    public class IgnoredParameterAttribute : Attribute
    {
        /// <summary>
        /// Gets whether this parameter is ignored. If set to true, this parameter should not be transferred to remote site.
        /// </summary>
        public bool IgnoredInParameter { get; }

        /// <summary>
        /// Gets whether the return is ignored. If set to true, this parameter should not be transferred back from the remote site.
        /// </summary>
        public bool IgnoredInReturn { get; }

        /// <summary>
        /// Initializes an instance of the IgnoredParameterAttribute.
        /// </summary>
        /// <param name="ignoredInParameter">Ignored in parameter. If set to true, this parameter should not be transferred to remote site.</param>
        /// <param name="ignoredInReturn">Ignored in return. If set to true, this parameter should not be transferred back from the remote site.</param>
        public IgnoredParameterAttribute(bool ignoredInParameter = true, bool ignoredInReturn = true)
        {
            IgnoredInParameter = ignoredInParameter;
            IgnoredInReturn = ignoredInReturn;
        }

        /// <summary>
        /// Initializes an instance of the IgnoredParameterAttribute, copying the value from <see cref="EventParameterIgnoredAttribute"/> object.
        /// </summary>
        /// <param name="attribute"><see cref="EventParameterIgnoredAttribute"/> object.</param>
        public IgnoredParameterAttribute(EventParameterIgnoredAttribute attribute)
        {
            IgnoredInParameter = attribute.IgnoredInParameter;
            IgnoredInReturn = attribute.IgnoredInReturn;
        }
    }
}
