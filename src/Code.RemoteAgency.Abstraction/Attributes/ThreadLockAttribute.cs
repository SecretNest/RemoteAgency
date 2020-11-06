using System;
using System.Collections.Generic;
using System.Text;

namespace SecretNest.RemoteAgency.Attributes
{
    /// <summary>
    /// Designate the task scheduler should be used while accessing assets within.
    /// </summary>
    /// <remarks><para>The default mode is <see cref="SecretNest.RemoteAgency.Attributes.ThreadLockMode.None"/> if this attribute is absent.</para>
    /// <para>In proxy, only event assets are affected.</para>
    /// <para>In service wrapper, method and property assets are affected.</para></remarks>
    /// <conceptualLink target="14c3caef-7392-4f68-b7eb-d0bb014a2e4c#InterfaceLevel" />
    /// <conceptualLink target="3c648b23-25dd-454c-b074-d0f3f0a0958c" />
    [AttributeUsage(AttributeTargets.Interface, Inherited = true, AllowMultiple = false)]
    public sealed class ThreadLockAttribute : Attribute
    {
        /// <summary>
        /// Gets thread choosing for accessing assets. 
        /// </summary>
        public ThreadLockMode ThreadLockMode { get; }

        /// <summary>
        /// Gets the name of the task scheduler to be used while accessing assets within. Only valid when <see cref="ThreadLockMode"/> is set as <see cref="SecretNest.RemoteAgency.Attributes.ThreadLockMode.TaskSchedulerSpecified"/>.
        /// </summary>
        /// <remarks>One task scheduler with the same name should be added to the instance of Remote Agency before processing the interface related.</remarks>
        public string TaskSchedulerName { get; }

        /// <summary>
        /// Initializes an instance of ThreadLockAttribute.
        /// </summary>
        /// <param name="threadLockMode">Thread lock mode. <see cref="SecretNest.RemoteAgency.Attributes.ThreadLockMode.TaskSchedulerSpecified"/> is not supported by this constructor.</param>
        public ThreadLockAttribute(ThreadLockMode threadLockMode)
        {
            if (threadLockMode == ThreadLockMode.TaskSchedulerSpecified)
            {
                throw new ArgumentOutOfRangeException(nameof(threadLockMode));
            }

            ThreadLockMode = threadLockMode;
        }

        /// <summary>
        /// Initializes an instance of ThreadLockAttribute with the name of the task scheduler specified.
        /// </summary>
        /// <param name="taskSchedulerName">Name of the task scheduler to be used while accessing assets within.</param>
        public ThreadLockAttribute(string taskSchedulerName)
        {
            ThreadLockMode = ThreadLockMode.TaskSchedulerSpecified;
            TaskSchedulerName = taskSchedulerName;
        }
    }

    /// <summary>
    /// Thread choosing for accessing assets.
    /// </summary>
    public enum ThreadLockMode
    {
        /// <summary>
        /// Not specified. The same thread of processing received message will be used to access asset specified.
        /// </summary>
        None,
        /// <summary>
        /// Always use SynchronizationContext to access assets within this object.
        /// </summary>
        SynchronizationContext,
        /// <summary>
        /// Always use one thread to access assets within this object.
        /// </summary>
        /// <remarks>This is not supported by neat version of Remote Agency due to lack of built-in SequentialScheduler.</remarks>
        AnyButSameThread,
        /// <summary>
        /// Always use the TaskScheduler specified by name to access assets within this object.
        /// </summary>
        TaskSchedulerSpecified
    }
}
