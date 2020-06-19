using System;
using System.Collections.Generic;
using System.Text;

namespace SecretNest.RemoteAgency.Attributes
{
    /// <summary>
    /// Declares the source thread(s) should be used while accessing assets within.
    /// </summary>
    /// <remarks>The default mode is <see cref="SecretNest.RemoteAgency.Attributes.ThreadLockMode.None"/> if this attribute is absent.</remarks>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, Inherited = true, AllowMultiple = false)]
    public class ThreadLockAttribute : Attribute
    {
        /// <summary>
        /// Gets thread choosing for accessing assets. 
        /// </summary>
        public ThreadLockMode ThreadLockMode { get; }

        /// <summary>
        /// Gets the name of the task scheduler to be used while accessing assets within. Only valid when <see cref="ThreadLockMode"/> is set to <see cref="SecretNest.RemoteAgency.Attributes.ThreadLockMode.TaskSchedulerSpecified"/>.
        /// </summary>
        /// <remarks>One task scheduler with the same name should be added to the instance of Remote Agency before accessing assets.</remarks>
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
        /// <param name="taskSchedulerName"></param>
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
        /// Not specified. The same thread of sending message to Remote Agency will be used to access asset specified.
        /// </summary>
        None,
        /// <summary>
        /// Always use SynchronizationContext to access assets within this object.
        /// </summary>
        SynchronizationContext,
        /// <summary>
        /// Always use one thread to access assets within this object.
        /// </summary>
        AnyButSameThread,
        /// <summary>
        /// Always use the TaskScheduler specified by name to access assets within this object.
        /// </summary>
        TaskSchedulerSpecified
    }
}
