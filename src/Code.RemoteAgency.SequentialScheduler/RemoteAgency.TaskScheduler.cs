using System;
using System.Collections.Generic;
using System.Text;
using SecretNest.RemoteAgency.Attributes;
using SecretNest.TaskSchedulers;

namespace SecretNest.RemoteAgency
{
    partial class RemoteAgency
    {
        /// <summary>
        /// Tries to add a task scheduler, which run tasks on a single thread, for accessing assets to the instance of Remote Agency.
        /// </summary>
        /// <param name="name">Name of the task scheduler.</param>
        /// <param name="taskScheduler">Created task scheduler.</param>
        /// <param name="waitForThread">Waiting for SequentialScheduler.Run() to provide thread. Default is <see langword="false"/>.</param>
        /// <returns>Result</returns>
        /// <remarks><p>When initializing with <paramref name="waitForThread"/> set to <see langword="false"/>, a free thread is created for this scheduler.</p>
        /// <p>When initializing with <paramref name="waitForThread"/> set to <see langword="true"/>, SequentialScheduler.Run() should be called from the thread which intends to be used for this scheduler before processing required by any interface.</p>
        /// <p>For details please refer to <see href="https://github.com/SecretNest/SequentialScheduler/">https://github.com/SecretNest/SequentialScheduler/</see>.</p>
        /// <p>This event is not present in Neat release.</p></remarks>
        /// <seealso cref="ThreadLockAttribute"/>
        public bool TryAddSequentialScheduler(string name, out SequentialScheduler taskScheduler, bool waitForThread = false)
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
