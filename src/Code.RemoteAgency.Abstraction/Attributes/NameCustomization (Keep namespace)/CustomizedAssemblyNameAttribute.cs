using System;
using System.Collections.Generic;
using System.Text;

namespace SecretNest.RemoteAgency.Attributes
{
    /// <summary>
    /// Specifies the name and namespace of the assembly generated. If this attribute is not present, the name and namespace will be chosen automatically.
    /// </summary>
    [AttributeUsage(AttributeTargets.Interface, Inherited = true, AllowMultiple = false)]
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
