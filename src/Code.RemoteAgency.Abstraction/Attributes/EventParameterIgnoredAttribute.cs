using System;
using System.Collections.Generic;
using System.Text;

namespace SecretNest.RemoteAgency.Attributes
{
    /// <summary>
    /// Specifies a parameter of the event should or should not be transferred to remote site. If this attribute absent, the default behavior is transferring all parameters.
    /// </summary>
    /// <remarks>You can also use <see cref="ParameterIgnoredAttribute"/> in declaration of the delegate related to this event, which has lower priority.</remarks>
    /// <seealso cref="ParameterIgnoredAttribute"/>
    /// <seealso cref="ParameterTwoWayPropertyAttribute"/>
    /// <seealso cref="EventParameterTwoWayPropertyAttribute"/>
    [AttributeUsage(AttributeTargets.Event, Inherited = true, AllowMultiple = true)]
    public class EventParameterIgnoredAttribute : Attribute
    {
        /// <summary>
        /// Parameter name of the event.
        /// </summary>
        public string ParameterName { get; }

        /// <summary>
        /// Ignored in parameter. If set to true, this parameter should not be transferred to remote site.
        /// </summary>
        public bool IgnoredInParameter { get; }

        /// <summary>
        /// Ignored in return. If set to true, this parameter should not be transferred back from the remote site.
        /// </summary>
        public bool IgnoredInReturn { get; }

        /// <summary>
        /// Initializes an instance of the EventParameterIgnoredAttribute.
        /// </summary>
        /// <param name="parameterName">Parameter name of the event.</param>
        /// <param name="ignoredInParameter">Ignored in parameter. If set to true, this parameter should not be transferred to remote site.</param>
        /// <param name="ignoredInReturn">Ignored in return. If set to true, this parameter should not be transferred back from the remote site.</param>
        public EventParameterIgnoredAttribute(string parameterName, bool ignoredInParameter = true, bool ignoredInReturn = true)
        {
            ParameterName = parameterName;
            IgnoredInParameter = ignoredInParameter;
            IgnoredInReturn = ignoredInReturn;
        }

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>A 32-bit signed integer hash code.</returns>
        /// <remarks>This is calculated based on <see cref="ParameterName"/>.</remarks>
        public override int GetHashCode()
        {
            return ParameterName.GetHashCode();
        }

        /// <summary>
        /// This API supports the product infrastructure and is not intended to be used directly from your code. Returns a value that indicates whether this instance is equal to a specified object.
        /// </summary>
        /// <param name="obj">An object to compare with this instance or null.</param>
        /// <returns>true if obj equals the type and value of this instance; otherwise, false.</returns>
        /// <remarks>This comparer is based on <see cref="ParameterName"/>.</remarks>
        public override bool Equals(object obj)
        {
            return obj is EventParameterIgnoredAttribute target && target.ParameterName == ParameterName;
        }
    }
}
