using System;
using System.Collections.Generic;
using System.Text;

namespace SecretNest.RemoteAgency.Attributes
{
    /// <summary>
    /// Specifies a parameter of the event contains a property or field which value should be send back to the caller.
    /// </summary>
    /// <remarks>You can also use <see cref="ParameterTwoWayPropertyAttribute"/> in declaration of the delegate related to this event, which has lower priority.</remarks>
    /// <seealso cref="ParameterTwoWayPropertyAttribute"/>
    [AttributeUsage(AttributeTargets.Event, Inherited = true, AllowMultiple = true)]
    public class EventParameterTwoWayPropertyAttribute : Attribute
    {
        /// <summary>
        /// Simple mode.
        /// </summary>
        /// <remarks>
        /// <list type="bullet">
        /// <item>
        ///     <term>True</term>
        ///     <description><see cref="ParameterProperty"/> is the name of property or field of the parameter entity.</description>
        /// </item>
        /// <item>
        ///     <term>False</term>
        ///     <description><see cref="ParameterProperty"/> is the path, starts with ".", from the parameter entity.</description>
        /// </item>
        /// </list>
        /// </remarks>
        public bool IsSimpleMode { get; }

        /// <summary>
        /// Parameter name of the event.
        /// </summary>
        public string ParameterName { get; }

        /// <summary>
        /// Parameter property.
        /// </summary>
        /// <remarks>
        /// Based on the value of <see cref="IsSimpleMode"/>:
        /// <list type="bullet">
        /// <item>
        ///     <term>True</term>
        ///     <description>The name of property or field of the parameter entity.</description>
        /// </item>
        /// <item>
        ///     <term>False</term>
        ///     <description>The path, starts with ".", from the parameter entity.</description>
        /// </item>
        /// </list>
        /// </remarks>
        public string ParameterProperty { get; }

        /// <summary>
        /// The type of the property or field specified by <see cref="ParameterProperty"/>.
        /// </summary>
        /// <remarks>When <see cref="IsSimpleMode"/> is set to true, this value will be ignored.</remarks>
        public Type ElementType { get; }

        /// <summary>
        /// Preferred property name in entity.
        /// </summary>
        /// <remarks>If it's set to null (default), the property name will be chosen automatically.</remarks>
        public string EntityPropertyName { get; }

        /// <summary>
        /// Initializes an instance of the EventParameterTwoWayPropertyAttribute. <see cref="IsSimpleMode"/> will be set to false.
        /// </summary>
        /// <param name="parameterName">Parameter name of the event.</param>
        /// <param name="parameterPropertyPath">The path, starts with ".", from the parameter entity.</param>
        /// <param name="elementType">The type of the property or field specified by <see cref="ParameterProperty"/>.</param>
        /// <param name="entityPropertyName">Preferred property name in entity. If it's set to null, the property name will be chosen automatically.</param>
        public EventParameterTwoWayPropertyAttribute(string parameterName, string parameterPropertyPath, Type elementType, string entityPropertyName)
        {
            ParameterName = parameterName;
            ParameterProperty = parameterPropertyPath;
            ElementType = elementType;
            EntityPropertyName = entityPropertyName;
            IsSimpleMode = false;
        }

        /// <summary>
        /// Initializes an instance of the EventParameterTwoWayPropertyAttribute. <see cref="IsSimpleMode"/> will be set to true.
        /// </summary>
        /// <param name="parameterName">Parameter name of the event.</param>
        /// <param name="parameterPropertyName">The name of property or field of the parameter entity.</param>
        /// <param name="entityPropertyName">Preferred property name in entity. If it's set to null (default), the property name will be chosen automatically.</param>
        public EventParameterTwoWayPropertyAttribute(string parameterName, string parameterPropertyName, string entityPropertyName = null)
        {
            ParameterName = parameterName;
            ParameterProperty = parameterPropertyName;
            EntityPropertyName = entityPropertyName;
            IsSimpleMode = true;
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
        /// <remarks>This comparer is based on <see cref="ParameterName"/>, <see cref="IsSimpleMode"/> and <see cref="ParameterProperty"/>.</remarks>
        public override bool Equals(object obj)
        {
            var target = obj as EventParameterTwoWayPropertyAttribute;
            return target != null && target.IsSimpleMode == IsSimpleMode && target.ParameterName == ParameterName && target.ParameterProperty == ParameterProperty;
        }
    }
}
