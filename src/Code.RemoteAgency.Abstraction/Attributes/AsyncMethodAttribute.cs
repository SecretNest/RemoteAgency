using System;
using System.Collections.Generic;
using System.Text;

namespace SecretNest.RemoteAgency.Attributes
{
    /// <summary>
    /// Specifies this method is asynchronous.
    /// </summary>
    /// <remarks>
    /// <para>The method marked with this attribute with <see cref="IsAsyncMethod"/> set as <see langword="true" /> must has a return value as Task, ValueTask or their derived class, or has no return. Method with other return type is not supported.</para>
    /// <para>Remote Agency only serialize and pass the result of the task, not the state of running task. Asynchronous is not supported.</para>
    /// </remarks>
    /// <conceptualLink target="14c3caef-7392-4f68-b7eb-d0bb014a2e4c#AssetLevel" />
    [AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public class AsyncMethodAttribute : Attribute
    {
        /// <summary>
        /// Gets whether this method is asynchronous.
        /// </summary>
        public bool IsAsyncMethod { get; }

        /// <summary>
        /// Initializes an instance of AsyncMethodAttribute.
        /// </summary>
        /// <param name="isAsyncMethod">Whether this method is asynchronous. Default value is <see langword="true" />.</param>
        public AsyncMethodAttribute(bool isAsyncMethod = true)
        {
            IsAsyncMethod = isAsyncMethod;
        }
    }
}
