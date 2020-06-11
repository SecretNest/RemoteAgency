using System;
using System.Collections.Generic;
using System.Text;
using SecretNest.RemoteAgency.Attributes;

namespace SecretNest.RemoteAgency
{
    /// <summary>
    /// Represents a mapping relation of a parameter and a property.
    /// </summary>
    /// <seealso cref="SerializingBehaviorAttributeBase"/>
    public class ValueMapping
    {
        /// <summary>
        /// Gets the full name of the type.
        /// </summary>
        public string TypeName { get; }

        /// <summary>
        /// Gets the unique name in the context.
        /// </summary>
        public string UniqueName { get; }

        /// <summary>
        /// Gets the name which can be represent in code.
        /// </summary>
        public string NameInCode { get; }

        /// <summary>
        /// Gets the name of the property in entity.
        /// </summary>
        public string PropertyName { get; }

        /// <summary>
        /// Metadata objects marked with derived class of <see cref="SerializingBehaviorAttributeBase"/> in parameter level.
        /// </summary>
        public IReadOnlyList<SerializingBehaviorAttributeBase> Attributes { get; }

        /// <summary>
        /// Metadata objects marked with derived class of <see cref="SerializingBehaviorAttributeBase"/> in parameter of the delegate of event. Only available when processing events.
        /// </summary>
        public IReadOnlyList<SerializingBehaviorAttributeBase> DelegateAttributes { get; }

        /// <summary>
        /// Initializes a new instance of the ValueMapping.
        /// </summary>
        /// <param name="uniqueName">Unique name in the context</param>
        /// <param name="propertyName">Name of the property in entity</param>
        /// <param name="typeName">Full name of the type</param>
        /// <param name="nameInCode">Name which can be represent in code</param>
        /// <param name="attributes">Metadata objects marked with derived class of <see cref="SerializingBehaviorAttributeBase"/> in parameter level.</param>
        /// <param name="delegateAttributes">Metadata objects marked with derived class of <see cref="SerializingBehaviorAttributeBase"/> in parameter of the delegate of event. Only available when processing events.</param>
        public ValueMapping(string uniqueName, string propertyName, string typeName, string nameInCode, IReadOnlyList<SerializingBehaviorAttributeBase> attributes, IReadOnlyList<SerializingBehaviorAttributeBase> delegateAttributes)
        {
            TypeName = typeName;
            UniqueName = uniqueName;
            PropertyName = propertyName;
            NameInCode = nameInCode;
            Attributes = attributes;
            DelegateAttributes = delegateAttributes;
        }
    }
}
