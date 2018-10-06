using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SecretNest.RemoteAgency
{
    /// <summary>
    /// Provides code generation for entity classes marked with [DataContract] and [DataMember]."/>.
    /// </summary>
    public class DataContractSerializerEntityCodeBuilder : EntityCodeBuilderBase
    {
        /// <summary>
        /// Generates code for an entity class.
        /// </summary>
        /// <param name="className">Entity class name</param>
        /// <param name="entityBaseTypeFullName">Type name of the base class of entity classes.</param>
        /// <param name="values">Values need to be represented in entity.</param>
        /// <param name="usedGenerics">Generic arguments should be represented in entity.</param>
        /// <param name="needValueBasedConstructor">Whether need to create a constructor.</param>
        /// <param name="sourceCodeBuilder">The <see cref="StringBuilder"/> for writing source code to.</param>
        /// <param name="valueBasedConstructorCallerCode">The calling code for create an instance of this entity class, starting with <code>= new ...</code>.</param>
        /// <returns>Class full name with generic arguments</returns>
        public override string BuildEntity(string className, string entityBaseTypeFullName, IEnumerable<ValueMapping> values, Dictionary<string, Type> usedGenerics, bool needValueBasedConstructor,
            StringBuilder sourceCodeBuilder, out string valueBasedConstructorCallerCode)
        {
            string resultName;

            sourceCodeBuilder.Append("[DataContract(Namespace = \"\")]\npublic class ").Append(className);
            if (usedGenerics != null && usedGenerics.Count > 0)
            {
                string code = string.Format("<{0}>", string.Join(", ", usedGenerics.Keys));
                sourceCodeBuilder.Append(code);
                resultName = className + code;
            }
            else
            {
                resultName = className;
            }
            sourceCodeBuilder.Append(" : ").Append(entityBaseTypeFullName);
            CodeBuilderHelper.ApplyConstraints(usedGenerics, sourceCodeBuilder);
            sourceCodeBuilder.AppendLine("\n{");
            foreach (var item in values)
            {
                sourceCodeBuilder.Append("[DataMember] public ").Append(item.TypeName).Append(" ").Append(item.PropertyName).AppendLine(" { get; set; }");
            }
            if (needValueBasedConstructor)
            {
                StringBuilder constructorCallerCodeBuilder = new StringBuilder(" = new ");
                constructorCallerCodeBuilder.Append(resultName).Append("(");
                sourceCodeBuilder.Append("public ").Append(className).Append("(");
                if (values.Any())
                {
                    var items = values.Select(i => i.TypeName + " " + i.UniqueName);
                    sourceCodeBuilder.Append(string.Join(", ", items));
                    constructorCallerCodeBuilder.Append(string.Join(", ", values.Select(i => i.NameInCode)));
                }
                constructorCallerCodeBuilder.AppendLine(");");
                valueBasedConstructorCallerCode = constructorCallerCodeBuilder.ToString();
                sourceCodeBuilder.AppendLine(")\n{");
                foreach (var item in values)
                {
                    sourceCodeBuilder.Append("this.").Append(item.PropertyName).Append(" = ").Append(item.UniqueName).AppendLine(";");
                }
                sourceCodeBuilder.AppendLine("}");
            }
            else
            {
                valueBasedConstructorCallerCode = null;
            }
            sourceCodeBuilder.AppendLine("}");
            return resultName;
        }
    }
}
