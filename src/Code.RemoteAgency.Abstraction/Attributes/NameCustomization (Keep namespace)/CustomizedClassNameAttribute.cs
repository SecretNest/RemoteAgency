﻿using System;
using System.Collections.Generic;
using System.Text;

namespace SecretNest.RemoteAgency.Attributes
{
    /// <summary>
    /// Specifies the name of proxy, service wrapper and assembly to be generated.
    /// </summary>
    /// <remarks>
    /// <para>This attribute cannot be inherited by derived classes and overriding members.</para>
    /// <para>When this attribute is not present, or name is set to <see langword="null"/> or empty string, the name not specified is chosen automatically.</para>
    /// </remarks>
    [AttributeUsage(AttributeTargets.Interface, Inherited = false, AllowMultiple = false)]
    public class CustomizedClassNameAttribute : Attribute
    {
        /// <summary>
        /// Gets the name of proxy class.
        /// </summary>
        /// <remarks>When the value is <see langword="null"/> or empty string, name is chosen automatically.</remarks>
        public string ProxyClassName { get; }

        /// <summary>
        /// Gets the name of service wrapper class.
        /// </summary>
        /// <remarks>When the value is <see langword="null"/> or empty string, name is chosen automatically.</remarks>
        public string ServiceWrapperClassName { get; }

        /// <summary>
        /// Gets the name of assembly.
        /// </summary>
        /// <remarks>When the value is <see langword="null"/> or empty string, name is chosen automatically.</remarks>
        public string AssemblyName { get; }

        /// <summary>
        /// Initializes an instance of the CustomizedAssemblyNameAttribute.
        /// </summary>
        /// <param name="proxyClassName">Name of proxy class. When the value is <see langword="null"/> or empty string, name is chosen automatically. Default value is <see langword="null"/>.</param>
        /// <param name="serviceWrapperClassName">Name of service wrapper class. When the value is <see langword="null"/> or empty string,name is chosen automatically. Default value is <see langword="null"/>.</param>
        /// <param name="assemblyName">Name of assembly. When the value is <see langword="null"/> or empty string, name is chosen automatically. Default value is <see langword="null"/>.</param>
        public CustomizedClassNameAttribute(string proxyClassName = null, string serviceWrapperClassName = null, string assemblyName = null)
        {
            ProxyClassName = proxyClassName;
            ServiceWrapperClassName = serviceWrapperClassName;
            AssemblyName = assemblyName;
        }
    }
}
