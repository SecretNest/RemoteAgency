using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using SecretNest.RemoteAgency.Attributes;
using SecretNest.RemoteAgency.TaskSchedulers;

namespace SecretNest.RemoteAgency
{
    public partial class RemoteAgency<TSerialized, TEntityBase>
    {
        private readonly ConcurrentDictionary<string, TaskScheduler> _namedTaskSchedulers =
            new ConcurrentDictionary<string, TaskScheduler>();

        /// <summary>
        /// Tries to add a task scheduler for accessing assets to the instance of Remote Agency.
        /// </summary>
        /// <param name="name">Name of the task scheduler.</param>
        /// <param name="taskScheduler">Task scheduler.</param>
        /// <returns>Result</returns>
        /// <seealso cref="ThreadLockAttribute"/>
        public bool TryAddTaskScheduler(string name, TaskScheduler taskScheduler)
        {
            return _namedTaskSchedulers.TryAdd(name, taskScheduler);
        }

        /// <summary>
        /// Tries to remove a task scheduler from the instance of Remote Agency.
        /// </summary>
        /// <param name="name">Name of the task scheduler.</param>
        /// <param name="removed">Removed task scheduler.</param>
        /// <returns>Result</returns>
        /// <seealso cref="ThreadLockAttribute"/>
        public bool TryRemoveTaskScheduler(string name, out TaskScheduler removed)
        {
            return _namedTaskSchedulers.TryRemove(name, out removed);
        }

        /// <summary>
        /// Tries to get a task scheduler from the instance of Remote Agency.
        /// </summary>
        /// <param name="name">Name of the task scheduler.</param>
        /// <param name="taskScheduler">Task scheduler.</param>
        /// <returns>Result</returns>
        /// <seealso cref="ThreadLockAttribute"/>
        public bool TryGetTaskScheduler(string name, out TaskScheduler taskScheduler)
        {
            return _namedTaskSchedulers.TryGetValue(name, out taskScheduler);
        }

        /// <summary>
        /// Tries to add a task scheduler, which run tasks on a single thread, for accessing assets to the instance of Remote Agency.
        /// </summary>
        /// <param name="name">Name of the task scheduler.</param>
        /// <param name="taskScheduler">Created task scheduler.</param>
        /// <param name="waitForThread">Waiting for <see cref="SequentialScheduler.Run"/> to provide thread. Default is <see langword="false"/>.</param>
        /// <returns>Result</returns>
        /// <remarks><p>When initializing with <paramref name="waitForThread"/> set to <see langword="false"/>, a free thread is created for this scheduler.</p>
        /// <p>When initializing with <paramref name="waitForThread"/> set to <see langword="true"/>, <see cref="SequentialScheduler.Run"/> should be called from the thread which intends to be used for this scheduler.</p></remarks>
        /// <seealso cref="ThreadLockAttribute"/>
        /// <seealso cref="SequentialScheduler"/>
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
