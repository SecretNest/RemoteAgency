using System;
using System.Collections.Generic;
using System.Text;

namespace SecretNest.RemoteAgency
{
    /// <summary>
    /// Represents a property to be created in entity class.
    /// </summary>
    public class EntityProperty
    {
        /// <summary>
        /// Gets the type of the property.
        /// </summary>
        public Type Type { get; }

        /// <summary>
        /// Gets the name of the property.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the metadata objects marked with derived class specified by <see cref="EntityCodeBuilderBase.ParameterLevelAttributeBaseType"/> in parameter level.
        /// </summary>
        /// <remarks>This will be set as <see langword="null"/> when <see cref="EntityCodeBuilderBase.ParameterLevelAttributeBaseType"/> is set as <see langword="null"/>.</remarks>
        public IReadOnlyList<EntityPropertyAttribute> Attributes { get; }

        /// <summary>
        /// Initializes an instance of EntityProperty.
        /// </summary>
        /// <param name="type">Type of the property.</param>
        /// <param name="name">Name of the property.</param>
        /// <param name="attributes">Metadata objects marked with derived class specified by <see cref="EntityCodeBuilderBase.ParameterLevelAttributeBaseType"/> in parameter level.</param>
        public EntityProperty(Type type, string name, IReadOnlyList<EntityPropertyAttribute> attributes)
        {
            Type = type;
            Name = name;
            Attributes = attributes;
        }
    }

    /// <summary>
    /// Represents an attribute that marked for this property.
    /// </summary>
    public class EntityPropertyAttribute
    {
        /// <summary>
        /// Gets the position where the attribute marked.
        /// </summary>
        public AttributePosition Position { get; }

        /// <summary>
        /// Gets the attribute marked.
        /// </summary>
        public Attribute Attribute { get; }

        /// <summary>
        /// Initializes an instance of EntityPropertyAttribute.
        /// </summary>
        /// <param name="position">Position where the attribute marked.</param>
        /// <param name="attribute">Attribute marked.</param>
        public EntityPropertyAttribute(AttributePosition position, Attribute attribute)
        {
            Position = position;
            Attribute = attribute;
        }
    }

    /// <summary>
    /// Contains a list of position where the attribute can be found.
    /// </summary>
    public enum AttributePosition
    {
        /// <summary>
        /// Attribute marked on return value.
        /// </summary>
        ReturnValue,
        /// <summary>
        /// Attribute marked on parameter.
        /// </summary>
        Parameter,
        /// <summary>
        /// Attribute marked on the field of the entity class defined the parameter.
        /// </summary>
        FieldOfParameter,
        /// <summary>
        /// Attribute marked on the property of the entity class defined the parameter.
        /// </summary>
        PropertyOfParameter,
        /// <summary>
        /// Attribute marked on the property of the helper class linked to the parameter.
        /// </summary>
        PropertyOfHelper,
        /// <summary>
        /// Attribute marked on the property as the asset in the interface.
        /// </summary>
        AssetProperty
    }
}
