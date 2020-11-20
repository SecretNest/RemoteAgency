using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SecretNest.RemoteAgency
{
    partial class RemoteAgencyBase
    {
        static TaskFactory _synchronizationContextTaskFactory;

        internal static TaskFactory GetSynchronizationContextTaskFactory()
        {
            //don't need to lock, optimized for performance.

            return _synchronizationContextTaskFactory ?? (_synchronizationContextTaskFactory =
                new TaskFactory(TaskScheduler.FromCurrentSynchronizationContext()));
        }

        /// <summary>
        /// Gets or sets the task factory based on synchronization context.
        /// </summary>
        /// <conceptualLink target="3c648b23-25dd-454c-b074-d0f3f0a0958c#SynchronizationContext" />
        public static TaskFactory SynchronizationContextTaskFactory
        {
            get => _synchronizationContextTaskFactory;
            set => _synchronizationContextTaskFactory = value;
        }

        /// <summary>
        /// Creates task factory based on synchronization context from current thread and sets it to <see cref="SynchronizationContextTaskFactory"/>.
        /// </summary>
        /// <exception cref="InvalidOperationException">The synchronization context cannot be obtained.</exception>
        /// <conceptualLink target="3c648b23-25dd-454c-b074-d0f3f0a0958c#SynchronizationContext" />
        public static void CreateSynchronizationContextTaskFactoryFromCurrentThread()
        {
            _synchronizationContextTaskFactory =
                new TaskFactory(TaskScheduler.FromCurrentSynchronizationContext());
        }
    }
}
