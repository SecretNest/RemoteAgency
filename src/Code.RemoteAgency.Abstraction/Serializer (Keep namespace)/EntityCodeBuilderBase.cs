using System;
using System.Collections.Generic;
using System.Text;
using SecretNest.RemoteAgency.Attributes;

namespace SecretNest.RemoteAgency
{
    /// <summary>
    /// Provides code generation for entity classes. This is an abstract class.
    /// </summary>
    public abstract class EntityCodeBuilderBase
    {
        /// <summary>
        /// Generates code for an entity class.
        /// </summary>
        /// <param name="className">Class name</param>
        /// <param name="entityBaseTypeFullName">Type name of the base class of entity classes.</param>
        /// <param name="fullNameOfIRemoteAgencyMessage">Type name of <see cref="IRemoteAgencyMessage"/>.</param>
        /// <param name="interfaceLevelAttributes">Metadata objects marked with derived class specified by <see cref="InterfaceLevelAttributeBaseType"/> in interface level.<remarks>This will contains nothing when <see cref="InterfaceLevelAttributeBaseType"/> is set to null.</remarks></param>
        /// <param name="assetLevelAttributes">Metadata objects marked with derived class specified by <see cref="AssetLevelAttributeBaseType"/> in asset level.<remarks>This will contains nothing when <see cref="AssetLevelAttributeBaseType"/> is set to null.</remarks></param>
        /// <param name="delegateLevelAttributes">Metadata objects marked with derived class specified by <see cref="DelegateLevelAttributeBaseType"/> for the delegate of event. Only available when processing events.<remarks>This will contains nothing when <see cref="DelegateLevelAttributeBaseType"/> is set to null.</remarks></param>
        /// <param name="values">Values need to be represented in entity.</param>
        /// <param name="usedGenerics">Generic arguments should be represented in entity.</param>
        /// <param name="needValueBasedConstructor">Whether need to create a constructor.</param>
        /// <param name="sourceCodeBuilder">The <see cref="StringBuilder"/> for writing source code to.</param>
        /// <param name="valueBasedConstructorCallerCode">The calling code for create an instance of this entity class, starting with <code>= new ...</code>.</param>
        /// <returns>Class code.</returns>
        public abstract string BuildEntity(string className, string entityBaseTypeFullName, string fullNameOfIRemoteAgencyMessage, 
            List<Attribute> interfaceLevelAttributes, List<Attribute> assetLevelAttributes, List<Attribute> delegateLevelAttributes,
            List<ValueMapping> values, Dictionary<string, Type> usedGenerics, bool needValueBasedConstructor,
            StringBuilder sourceCodeBuilder, out string valueBasedConstructorCallerCode);

        /// <summary>
        /// Gets the type of the base class of attributes which are used to mark metadata on interface level.
        /// </summary>
        public abstract Type InterfaceLevelAttributeBaseType { get; }

        /// <summary>
        /// Gets the type of the base class of attributes which are used to mark metadata on asset level.
        /// </summary>
        public abstract Type AssetLevelAttributeBaseType { get; }

        /// <summary>
        /// Gets the type of the base class of attributes which are used to mark metadata on delegate level. Only works with processing events.
        /// </summary>
        public abstract Type DelegateLevelAttributeBaseType { get; }

        /// <summary>
        /// Gets the type of the base class of attributes which are used to mark metadata on parameter level.
        /// </summary>
        public abstract Type ParameterLevelAttributeBaseType { get; }

        /// <summary>
        /// Creates an empty message which is allowed to be serialized.
        /// </summary>
        /// <returns>Empty message.</returns>
        public abstract IRemoteAgencyMessage CreateEmptyMessage();
    }
}
