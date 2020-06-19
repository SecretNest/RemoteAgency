using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace SecretNest.RemoteAgency.Attributes
{
    /// <summary>
    /// Specifies the parameter should or should not be transferred to remote site. If this attribute absent, the default behavior is transferring all parameters.
    /// </summary>
    /// <remarks>When <see cref="IsIgnoredFromParameter"/> or <see cref="IsIgnoredFromReturn"/> is set to true, the <see cref="ParameterTwoWayPropertyAttribute"/> and <see cref="EventParameterTwoWayPropertyAttribute"/> will be ignored.</remarks>
    /// <seealso cref="EventParameterIgnoredAttribute"/>
    /// <seealso cref="ParameterTwoWayPropertyAttribute"/>
    /// <seealso cref="EventParameterTwoWayPropertyAttribute"/>
    [AttributeUsage(AttributeTargets.Parameter, Inherited = true, AllowMultiple = false)]
    public class ParameterIgnoredAttribute : Attribute
    {
        /// <summary>
        /// Gets whether this parameter is excluded from parameter entity. If set to true, this parameter should not be transferred to remote site.
        /// </summary>
        public bool IsIgnoredFromParameter { get; }

        /// <summary>
        /// Gets whether this parameter is excluded from returning entity. If set to true, this parameter should not be transferred back from the remote site.
        /// </summary>
        public bool IsIgnoredFromReturn { get; }

        /// <summary>
        /// Initializes an instance of the ParameterIgnoredAttribute.
        /// </summary>
        /// <param name="ignoredFromParameter">Ignored from parameter. If set to true, this parameter should not be transferred to remote site.</param>
        /// <param name="ignoredFromReturn">Ignored from return. If set to true, this parameter should not be transferred back from the remote site.</param>
        public ParameterIgnoredAttribute(bool ignoredFromParameter = true, bool ignoredFromReturn = true)
        {
            IsIgnoredFromParameter = ignoredFromParameter;
            IsIgnoredFromReturn = ignoredFromReturn;
        }

        /// <summary>
        /// Initializes an instance of the IgnoredParameterAttribute, copying the value from <see cref="EventParameterIgnoredAttribute"/> object.
        /// </summary>
        /// <param name="attribute"><see cref="EventParameterIgnoredAttribute"/> object.</param>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public ParameterIgnoredAttribute(EventParameterIgnoredAttribute attribute)
        {
            IsIgnoredFromParameter = attribute.IsIgnoredFromParameter;
            IsIgnoredFromReturn = attribute.IsIgnoredFromReturn;
        }
    }
}
