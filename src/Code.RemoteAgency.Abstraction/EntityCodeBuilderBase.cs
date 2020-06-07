using System;
using System.Collections.Generic;
using System.Text;

namespace SecretNest.RemoteAgency
{
#warning Need to redesign

    /// <summary>
    /// Provides code generation for entity classes. This is an abstract class.
    /// </summary>
    /// <seealso cref="Attributes.CustomizedEventParameterEntityPropertyNameAttribute"/>
    /// <seealso cref="Attributes.CustomizedParameterEntityAttribute"/>
    /// <seealso cref="Attributes.CustomizedParameterEntityPropertyNameAttribute"/>
    /// <seealso cref="Attributes.CustomizedReturnEntityAttribute"/>
    /// <seealso cref="Attributes.CustomizedReturnEntityPropertyNameAttribute"/>
    /// <seealso cref="Attributes.CustomizedPropertyGetRequestEntityAttribute"/>
    /// <seealso cref="Attributes.CustomizedPropertyGetResponseEntityAttribute"/>
    /// <seealso cref="Attributes.CustomizedPropertyGetResponsePropertyNameAttribute"/>
    /// <seealso cref="Attributes.CustomizedPropertySetRequestEntityAttribute"/>
    /// <seealso cref="Attributes.CustomizedPropertySetRequestPropertyNameAttribute"/>
    /// <seealso cref="Attributes.CustomizedPropertySetResponseEntityAttribute"/>
    /// <seealso cref="Attributes.EventParameterTwoWayPropertyAttribute"/>
    /// <seealso cref="Attributes.ParameterTwoWayPropertyAttribute"/>
    public abstract class EntityCodeBuilderBase
    {
        /// <summary>
        /// Generates code for an entity class.
        /// </summary>
        /// <param name="className">Class name</param>
        /// <param name="entityBaseTypeFullName">Type name of the base class of entity classes.</param>
        /// <param name="values">Values need to be represented in entity.</param>
        /// <param name="usedGenerics">Generic arguments should be represented in entity.</param>
        /// <param name="needValueBasedConstructor">Whether need to create a constructor.</param>
        /// <param name="sourceCodeBuilder">The <see cref="StringBuilder"/> for writing source code to.</param>
        /// <param name="valueBasedConstructorCallerCode">The calling code for create an instance of this entity class, starting with <code>= new ...</code>.</param>
        /// <returns>Class full name with generic arguments</returns>
        public abstract string BuildEntity(string className, string entityBaseTypeFullName, IEnumerable<ValueMapping> values, Dictionary<string, Type> usedGenerics, bool needValueBasedConstructor,
            StringBuilder sourceCodeBuilder, out string valueBasedConstructorCallerCode);
    }
}
