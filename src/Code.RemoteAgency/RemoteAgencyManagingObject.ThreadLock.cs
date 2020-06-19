using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SecretNest.RemoteAgency.Attributes;
using SecretNest.RemoteAgency.TaskSchedulers;

namespace SecretNest.RemoteAgency
{
    partial class RemoteAgencyManagingObject
    {
        private SequentialScheduler _sequentialScheduler = null;
        private TaskFactory _taskFactory = null;

        protected delegate IRemoteAgencyMessage AccessWithReturn(IRemoteAgencyMessage message, out Exception exception);
        protected delegate void AccessWithoutReturn(IRemoteAgencyMessage message);

        delegate void AccessWithReturnCaller(AccessWithReturn callback, IRemoteAgencyMessage message, out IRemoteAgencyMessage response, out Exception exception);
        delegate void AccessWithoutReturnCaller(AccessWithoutReturn callback, IRemoteAgencyMessage message, out Exception exception);
        
        delegate bool TryGetTaskSchedulerCallback(string name, out TaskScheduler taskScheduler);

        void InitializeThreadLock(ThreadLockMode threadLockMode)
        {
            switch (threadLockMode)
            {
                case ThreadLockMode.None:
                    _processThreadLockWithReturn = ProcessWithNoThreadLock;
                    _processThreadLockWithoutReturn = ProcessWithNoThreadLock;
                    break;
                case ThreadLockMode.SynchronizationContext:
                    _processThreadLockWithReturn = ProcessWithSynchronizationContext;
                    _processThreadLockWithoutReturn = ProcessWithSynchronizationContext;
                    break;
                case ThreadLockMode.AnyButSameThread:
                    _sequentialScheduler = new SequentialScheduler();
                    _taskFactory = new TaskFactory(_sequentialScheduler);
                    _processThreadLockWithReturn = ProcessWithTaskScheduler;
                    _processThreadLockWithoutReturn = ProcessWithTaskScheduler;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(threadLockMode), threadLockMode, null);
            }
        }

        void InitializeThreadLock(string threadLockTaskSchedulerName, TryGetTaskSchedulerCallback tryGetTaskSchedulerCallback)
        {
            if (!tryGetTaskSchedulerCallback(threadLockTaskSchedulerName, out var selectedTaskScheduler))
            {
                _taskFactory = new TaskFactory(selectedTaskScheduler);
                _processThreadLockWithReturn = ProcessWithTaskScheduler;
                _processThreadLockWithoutReturn = ProcessWithTaskScheduler;
            }
            else
            {
                throw new InvalidOperationException($"Task scheduler {threadLockTaskSchedulerName} is not found in this Remote Agency instance.");
            }
        }

        void DisposeThreadLock()
        {
            if (_sequentialScheduler != null)
            {
                _sequentialScheduler.Dispose();
                _sequentialScheduler = null;
            }

            _taskFactory = null;
            _processThreadLockWithReturn = null;
            _processThreadLockWithoutReturn = null;
        }
        
        private AccessWithReturnCaller _processThreadLockWithReturn;
        private AccessWithoutReturnCaller _processThreadLockWithoutReturn;

        protected void ProcessThreadLockWithReturn(
            AccessWithReturn callback, IRemoteAgencyMessage message,
            out IRemoteAgencyMessage response, out Exception exception)
            => _processThreadLockWithReturn(callback, message, out response, out exception);

        protected void ProcessThreadLockWithoutReturn(
            AccessWithoutReturn callback, IRemoteAgencyMessage message, out Exception exception)
            => _processThreadLockWithoutReturn(callback, message, out exception);

        void ProcessWithNoThreadLock(AccessWithReturn callback, IRemoteAgencyMessage message, out IRemoteAgencyMessage response, out Exception exception)
        {
            response = callback(message, out exception);
        }

        void ProcessWithNoThreadLock(AccessWithoutReturn callback, IRemoteAgencyMessage message, out Exception exception)
        {
            try
            {
                callback(message);
                exception = default;
            }
            catch (Exception ex)
            {
                exception = ex;
            }
        }

        void ProcessWithSynchronizationContext(AccessWithReturn callback, IRemoteAgencyMessage message, out IRemoteAgencyMessage response, out Exception exception)
        {
            ProcessWithSynchronizationContextEntityWithResponse state =
                new ProcessWithSynchronizationContextEntityWithResponse()
                {
                    Callback = callback,
                    Message = message
                };
            SynchronizationContext.Current.Post(ProcessWithSynchronizationContextWithResponseInternal, state);
            response = state.Response;
            exception = state.Exception;
        }

        void ProcessWithSynchronizationContextWithResponseInternal(object state)
        {
            ((ProcessWithSynchronizationContextEntityWithResponse) state).Response =
                ((ProcessWithSynchronizationContextEntityWithResponse) state).Callback(
                    ((ProcessWithSynchronizationContextEntityWithResponse) state).Message,
                    out ((ProcessWithSynchronizationContextEntityWithResponse) state).Exception);
        }

        class ProcessWithSynchronizationContextEntityWithResponse
        {
            public AccessWithReturn Callback;
            public IRemoteAgencyMessage Message;
            public IRemoteAgencyMessage Response;
            public Exception Exception;
        }

        void ProcessWithSynchronizationContext(AccessWithoutReturn callback, IRemoteAgencyMessage message, out Exception exception)
        {
            ProcessWithSynchronizationContextEntity state =
                new ProcessWithSynchronizationContextEntity()
                {
                    Callback = callback,
                    Message = message
                };
            SynchronizationContext.Current.Post(ProcessWithSynchronizationContextInternal, state);
            exception = state.Exception;
        }

        void ProcessWithSynchronizationContextInternal(object state)
        {
            try
            {
                ((ProcessWithSynchronizationContextEntity) state).Callback(
                    ((ProcessWithSynchronizationContextEntity) state).Message);
                ((ProcessWithSynchronizationContextEntity) state).Exception = null;
            }
            catch (Exception ex)
            {
                ((ProcessWithSynchronizationContextEntity) state).Exception = ex;
            }
        }

        class ProcessWithSynchronizationContextEntity
        {
            public AccessWithoutReturn Callback;
            public IRemoteAgencyMessage Message;
            public Exception Exception;
        }

        void ProcessWithTaskScheduler(AccessWithReturn callback, IRemoteAgencyMessage message, out IRemoteAgencyMessage response, out Exception exception)
        {
            try
            {
                var result = _taskFactory
                    .StartNew(() => ProcessWithTaskSchedulerWithResponseInternal(callback, message)).Result;
                response = result.Item1;
                exception = result.Item2;
            }
            catch (TaskCanceledException)
            {
                response = default;
                exception = default;
            }
        }

        Tuple<IRemoteAgencyMessage, Exception> ProcessWithTaskSchedulerWithResponseInternal(
            AccessWithReturn callback, IRemoteAgencyMessage message)
        {
            var response = callback(message, out var exception);
            return new Tuple<IRemoteAgencyMessage, Exception>(response, exception);
        }

        void ProcessWithTaskScheduler(AccessWithoutReturn callback, IRemoteAgencyMessage message, out Exception exception)
        {
            try
            {
                exception = _taskFactory
                    .StartNew(() => ProcessWithTaskSchedulerInternal(callback, message)).Result;
            }
            catch (TaskCanceledException)
            {
                exception = default;
            }
        }

        Exception ProcessWithTaskSchedulerInternal(AccessWithoutReturn callback, IRemoteAgencyMessage message)
        {
            try
            {
                callback(message);
                return null;
            }
            catch (Exception ex)
            {
                return ex;
            }
        }
    }
}
