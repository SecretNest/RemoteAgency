using System;
using System.Collections.Generic;
using System.Text;
using SecretNest.RemoteAgency.Attributes;

namespace SecretNest.RemoteAgency
{
    /// <summary>
    /// Provides code generation for entity classes. This is an abstract class.
    /// </summary>
    /// <seealso cref="SerializingBehaviorAttributeBase"/>
    public abstract class EntityCodeBuilderBase
    {
        /// <summary>
        /// Generates code for an entity class.
        /// </summary>
        /// <param name="className">Class name</param>
        /// <param name="entityBaseTypeFullName">Type name of the base class of entity classes.</param>
        /// <param name="messagePackageInterfaceTypeFullName">Type name of <see cref="IRemoteAgencyMessage{TSerialized}"/>.</param>
        /// <param name="interfaceLevelAttributes">Metadata objects marked with derived class of <see cref="SerializingBehaviorAttributeBase"/> in interface level.</param>
        /// <param name="assetLevelAttributes">Metadata objects marked with derived class of <see cref="SerializingBehaviorAttributeBase"/> in asset level.</param>
        /// <param name="delegateLevelAttributes">Metadata objects marked with derived class of <see cref="SerializingBehaviorAttributeBase"/> for the delegate of event. Only available when processing events.</param>
        /// <param name="values">Values need to be represented in entity.</param>
        /// <param name="usedGenerics">Generic arguments should be represented in entity.</param>
        /// <param name="needValueBasedConstructor">Whether need to create a constructor.</param>
        /// <param name="sourceCodeBuilder">The <see cref="StringBuilder"/> for writing source code to.</param>
        /// <param name="valueBasedConstructorCallerCode">The calling code for create an instance of this entity class, starting with <code>= new ...</code>.</param>
        /// <returns>Class code.</returns>
        public abstract string BuildEntity(string className, string entityBaseTypeFullName, string messagePackageInterfaceTypeFullName,
            List<SerializingBehaviorAttributeBase> interfaceLevelAttributes, List<SerializingBehaviorAttributeBase> assetLevelAttributes, List<SerializingBehaviorAttributeBase> delegateLevelAttributes,
            IEnumerable<ValueMapping> values, Dictionary<string, Type> usedGenerics, bool needValueBasedConstructor,
            StringBuilder sourceCodeBuilder, out string valueBasedConstructorCallerCode);
    }
}
