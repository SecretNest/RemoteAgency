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
        /// <remarks>This will contains nothing when <see cref="EntityCodeBuilderBase.ParameterLevelAttributeBaseType"/> is set to null.</remarks>
        public IReadOnlyList<Attribute> Attributes { get; }

        /// <summary>
        /// Gets the metadata objects marked with derived class specified by <see cref="EntityCodeBuilderBase.DelegateLevelAttributeBaseType"/> in parameter of the delegate of event. Only available when processing events.
        /// </summary>
        /// <remarks>This will contains nothing when <see cref="EntityCodeBuilderBase.DelegateLevelAttributeBaseType"/> is set to null.</remarks>
        public IReadOnlyList<Attribute> DelegateAttributes { get; } 

        /// <summary>
        /// Initializes an instance of EntityProperty.
        /// </summary>
        /// <param name="type">Type of the property.</param>
        /// <param name="name">Name of the property.</param>
        /// <param name="attributes">Metadata objects marked with derived class specified by <see cref="EntityCodeBuilderBase.ParameterLevelAttributeBaseType"/> in parameter level.</param>
        /// <param name="delegateAttributes">Metadata objects marked with derived class specified by <see cref="EntityCodeBuilderBase.DelegateLevelAttributeBaseType"/> in parameter of the delegate of event. Only available when processing events.</param>
        public EntityProperty(Type type, string name, IReadOnlyList<Attribute> attributes, IReadOnlyList<Attribute> delegateAttributes)
        {
            Type = type;
            Name = name;
            Attributes = attributes;
            DelegateAttributes = delegateAttributes;
        }
    }
}
