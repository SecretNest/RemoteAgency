using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace SecretNest.RemoteAgency.Attributes
{
    /// <summary>
    /// Specifies the parameter contains a property or field which value should be send back to the caller.
    /// </summary>
    /// <remarks><para>When a parameter contains properties or fields which will be changed on the target site and need to be sent back to the caller, use <see cref="ParameterTwoWayPropertyAttribute"/> on related properties.</para>
    /// <para>When a parameter marked with "ref / ByRef", the value of the parameter will be passed back to the caller. Due to lack of tracking information, regardless of whether this parameter contains changed properties or fields, the whole object will be transferred and replaced. If this is not the expected operation, use <see cref="ParameterTwoWayPropertyAttribute"/> on related properties and fields instead of marking "ref / ByRef".</para>
    /// </remarks>
    /// <seealso cref="EventParameterTwoWayPropertyAttribute"/>
    [AttributeUsage(AttributeTargets.Parameter, Inherited = true, AllowMultiple = true)]
    public class ParameterTwoWayPropertyAttribute : Attribute
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
        /// Initializes an instance of the ParameterTwoWayPropertyAttribute. <see cref="IsSimpleMode"/> will be set to false.
        /// </summary>
        /// <param name="parameterPropertyPath">The path, starts with ".", from the parameter entity.</param>
        /// <param name="elementType">The type of the property or field specified by <see cref="ParameterProperty"/>.</param>
        /// <param name="entityPropertyName">Preferred property name in entity. If it's set to null, the property name will be chosen automatically.</param>
        public ParameterTwoWayPropertyAttribute(string parameterPropertyPath, Type elementType, string entityPropertyName)
        {
            ParameterProperty = parameterPropertyPath;
            ElementType = elementType;
            EntityPropertyName = entityPropertyName;
            IsSimpleMode = false;
        }

        /// <summary>
        /// Initializes an instance of the ParameterTwoWayPropertyAttribute. <see cref="IsSimpleMode"/> will be set to true.
        /// </summary>
        /// <param name="parameterPropertyName">The name of property or field of the parameter entity.</param>
        /// <param name="entityPropertyName">Preferred property name in entity. If it's set to null (default), the property name will be chosen automatically.</param>
        public ParameterTwoWayPropertyAttribute(string parameterPropertyName, string entityPropertyName = null)
        {
            ParameterProperty = parameterPropertyName;
            EntityPropertyName = entityPropertyName;
            IsSimpleMode = true;
        }

        /// <summary>
        /// Initializes an instance of the ParameterTwoWayPropertyAttribute, copying the value from <see cref="EventParameterTwoWayPropertyAttribute"/> object.
        /// </summary>
        /// <param name="attribute"><see cref="EventParameterTwoWayPropertyAttribute"/> object.</param>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public ParameterTwoWayPropertyAttribute(EventParameterTwoWayPropertyAttribute attribute)
        {
            ParameterProperty = attribute.ParameterProperty;
            ElementType = attribute.ElementType;
            EntityPropertyName = attribute.EntityPropertyName;
            IsSimpleMode = attribute.IsSimpleMode;
        }

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>A 32-bit signed integer hash code.</returns>
        /// <remarks>This is calculated based on <see cref="ParameterProperty"/>.</remarks>
        public override int GetHashCode()
        {
            return ParameterProperty.GetHashCode();
        }

        /// <summary>
        /// This API supports the product infrastructure and is not intended to be used directly from your code. Returns a value that indicates whether this instance is equal to a specified object.
        /// </summary>
        /// <param name="obj">An object to compare with this instance or null.</param>
        /// <returns>true if obj equals the type and value of this instance; otherwise, false.</returns>
        /// <remarks>This comparer is based on <see cref="IsSimpleMode"/> and <see cref="ParameterProperty"/>.</remarks>
        public override bool Equals(object obj)
        {
            return obj is ParameterTwoWayPropertyAttribute target && target.IsSimpleMode == IsSimpleMode && target.ParameterProperty == ParameterProperty;
        }
    }
}
