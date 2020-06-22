using System;
using System.Collections.Generic;
using System.Text;

namespace SecretNest.RemoteAgency.Attributes
{
    /// <summary>
    /// Specifies the name and namespace of the assembly generated.
    /// </summary>
    /// <remarks><para>When this attribute is not present, or <see cref="AssemblyName"/> or <see cref="Namespace"/> is set to <see langword="null"/> or empty string, the value will be chosen automatically.</para>
    /// <para>This attribute cannot be inherited by derived classes and overriding members.</para>
    /// </remarks>
    [AttributeUsage(AttributeTargets.Interface, Inherited = false, AllowMultiple = false)]
    public class CustomizedAssemblyNameAttribute : Attribute
    {
        /// <summary>
        /// Gets the assembly name.
        /// </summary>
        public string AssemblyName { get; }

        /// <summary>
        /// Gets the namespace.
        /// </summary>
        public string Namespace { get; }

        /// <summary>
        /// Initializes an instance of the CustomizedAssemblyNameAttribute.
        /// </summary>
        /// <param name="assemblyName">Assembly name.</param>
        /// <param name="namespace">Namespace.</param>
        public CustomizedAssemblyNameAttribute(string assemblyName, string @namespace)
        {
            AssemblyName = assemblyName;
            Namespace = @namespace;
        }
    }
}
