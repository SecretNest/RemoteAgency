using SecretNest.RemoteAgency.Attributes;
using SecretNest.TaskSchedulers;

namespace SecretNest.RemoteAgency
{
    partial class RemoteAgencyBase
    {
        /// <summary>
        /// Tries to create and add a task scheduler, which run tasks on a single thread, for accessing assets to the instance of Remote Agency.
        /// </summary>
        /// <param name="name">Name of the task scheduler.</param>
        /// <param name="taskScheduler">Created task scheduler. This value should be disposed by user code.</param>
        /// <param name="waitForThread">Waiting for SequentialScheduler.Run() to provide thread. Default is <see langword="false"/>.</param>
        /// <returns>Result</returns>
        /// <remarks><para>When initializing with <paramref name="waitForThread"/> set to <see langword="false"/>, a free thread is created for this scheduler.</para>
        /// <para>When initializing with <paramref name="waitForThread"/> set to <see langword="true"/>, SequentialScheduler.Run() should be called from the thread which intends to be used for this scheduler before processing required by any interface.</para>
        /// <para>Caution: The object specified by <paramref name="taskScheduler"/> should be disposed by user code if <see langword="true"/> is returned.</para>
        /// <para>For details please refer to <see href="https://github.com/SecretNest/SequentialScheduler/">https://github.com/SecretNest/SequentialScheduler/</see>.</para>
        /// <para>This event is not present in Neat release.</para></remarks>
        /// <seealso cref="ThreadLockAttribute"/>
        public bool TryCreateAddSequentialScheduler(string name, out SequentialScheduler taskScheduler, bool waitForThread = false)
        {
            if (_namedTaskSchedulers.ContainsKey(name))
            {
                taskScheduler = default;
                return false;
            }

            taskScheduler = new SequentialScheduler(waitForThread);
            if (TryAddTaskScheduler(name, taskScheduler))
            {
                return true;
            }

            taskScheduler.Dispose();
            taskScheduler = default;
            return false;
        }
    }
}
