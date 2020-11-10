using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using SecretNest.RemoteAgency.Attributes;

namespace SecretNest.RemoteAgency
{
    partial class RemoteAgencyBase
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


    }
}
