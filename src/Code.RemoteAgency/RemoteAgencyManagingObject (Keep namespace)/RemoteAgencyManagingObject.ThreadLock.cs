using SecretNest.RemoteAgency.Attributes;
using System;
using System.Threading.Tasks;

namespace SecretNest.RemoteAgency
{
    partial class RemoteAgencyManagingObject
    {

        #region Entry methods
        protected void ProcessThreadLockWithReturn(
            AccessWithReturn callback, IRemoteAgencyMessage message,
            out IRemoteAgencyMessage response, out Exception exception,
            out LocalExceptionHandlingMode localExceptionHandlingMode)
            => _processThreadLockWithReturn(callback, message, out response, out exception,
                out localExceptionHandlingMode);

        protected void ProcessThreadLockWithoutReturn(
            AccessWithoutReturn callback, IRemoteAgencyMessage message, out Exception exception,
            out LocalExceptionHandlingMode localExceptionHandlingMode)
            => _processThreadLockWithoutReturn(callback, message, out exception,
                out localExceptionHandlingMode);

        protected void CallCloseRequestedByManagingObject(AccessCloseRequestedByManagingObject callback, bool sendSpecialCommand)
            => _callCloseRequestedByManagingObjectCaller(callback, sendSpecialCommand);

        protected void CallOnRemoteClosed(AccessOnRemoteClosed callback, Guid siteId, Guid? instanceId)
            => _callOnRemoteClosed(callback, siteId, instanceId);

        #endregion

        #region Entry methods delegate and maintenance

        private AccessWithReturnCaller _processThreadLockWithReturn;
        private AccessWithoutReturnCaller _processThreadLockWithoutReturn;
        private CallCloseRequestedByManagingObjectCaller _callCloseRequestedByManagingObjectCaller;
        private CallOnRemoteClosedCaller _callOnRemoteClosed;

        protected delegate IRemoteAgencyMessage AccessWithReturn(IRemoteAgencyMessage message, out Exception exception,
            out LocalExceptionHandlingMode localExceptionHandlingMode);
        protected delegate void AccessWithoutReturn(IRemoteAgencyMessage message,
            out LocalExceptionHandlingMode localExceptionHandlingMode);
        protected delegate void AccessCloseRequestedByManagingObject(bool sendSpecialCommand);
        protected delegate void AccessOnRemoteClosed(Guid siteId, Guid? instanceId);

        delegate void AccessWithReturnCaller(AccessWithReturn callback, IRemoteAgencyMessage message,
            out IRemoteAgencyMessage response, out Exception exception,
            out LocalExceptionHandlingMode localExceptionHandlingMode);
        delegate void AccessWithoutReturnCaller(AccessWithoutReturn callback, IRemoteAgencyMessage message,
            out Exception exception, out LocalExceptionHandlingMode localExceptionHandlingMode);
        delegate void CallCloseRequestedByManagingObjectCaller(AccessCloseRequestedByManagingObject callback, bool sendSpecialCommand);
        delegate void CallOnRemoteClosedCaller(AccessOnRemoteClosed callback, Guid siteId, Guid? instanceId);

        void InitializeThreadLock(ThreadLockMode threadLockMode)
        {
            switch (threadLockMode)
            {
                case ThreadLockMode.None:
                    _processThreadLockWithReturn = ProcessWithNoThreadLock;
                    _processThreadLockWithoutReturn = ProcessWithNoThreadLock;
                    _callCloseRequestedByManagingObjectCaller = CallCloseRequestedByManagingObjectWithNoThreadLock;
                    _callOnRemoteClosed = CallOnRemoteClosedWithNoThreadLock;
                    break;
                case ThreadLockMode.SynchronizationContext:
                    _taskFactory = RemoteAgencyBase.GetSynchronizationContextTaskFactory();
                    _processThreadLockWithReturn = ProcessWithTaskScheduler;
                    _processThreadLockWithoutReturn = ProcessWithTaskScheduler;
                    _callCloseRequestedByManagingObjectCaller = CallCloseRequestedByManagingObjectWithTaskScheduler;
                    _callOnRemoteClosed = CallOnRemoteClosedWithTaskScheduler;
                    break;
                case ThreadLockMode.AnyButSameThread:
                    PrepareSequentialScheduler();
                    _processThreadLockWithReturn = ProcessWithTaskScheduler;
                    _processThreadLockWithoutReturn = ProcessWithTaskScheduler;
                    _callCloseRequestedByManagingObjectCaller = CallCloseRequestedByManagingObjectWithTaskScheduler;
                    _callOnRemoteClosed = CallOnRemoteClosedWithTaskScheduler;
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
                _callCloseRequestedByManagingObjectCaller = CallCloseRequestedByManagingObjectWithTaskScheduler;
                _callOnRemoteClosed = CallOnRemoteClosedWithTaskScheduler;
            }
            else
            {
                throw new InvalidOperationException($"Task scheduler {threadLockTaskSchedulerName} is not found in this Remote Agency instance.");
            }
        }

        void DisposeThreadLock()
        {
            _taskFactory = null;
            _processThreadLockWithReturn = null;
            _processThreadLockWithoutReturn = null;
            _callCloseRequestedByManagingObjectCaller = null;
            _callOnRemoteClosed = null;

            DisposeSequentialScheduler();
        }
        #endregion

        #region No Lock
        void ProcessWithNoThreadLock(AccessWithReturn callback, IRemoteAgencyMessage message, out IRemoteAgencyMessage response, out Exception exception, out LocalExceptionHandlingMode localExceptionHandlingMode)
        {
            response = callback(message, out exception, out localExceptionHandlingMode);
        }

        void ProcessWithNoThreadLock(AccessWithoutReturn callback, IRemoteAgencyMessage message, out Exception exception, out LocalExceptionHandlingMode localExceptionHandlingMode)
        {
            localExceptionHandlingMode = LocalExceptionHandlingMode.Redirect;
            try
            {
                callback(message, out localExceptionHandlingMode);
                exception = default;
            }
#pragma warning disable CA1031 // Do not catch general exception types
            catch (Exception ex)
            {
                exception = ex;
            }
#pragma warning restore CA1031 // Do not catch general exception types
        }

        void CallCloseRequestedByManagingObjectWithNoThreadLock(AccessCloseRequestedByManagingObject callback, bool sendSpecialCommand)
        {
            callback(sendSpecialCommand);
        }

        void CallOnRemoteClosedWithNoThreadLock(AccessOnRemoteClosed callback, Guid siteId, Guid? instanceId)
        {
            callback(siteId, instanceId);
        }

        #endregion

        #region TaskScheduelr
        private TaskFactory _taskFactory;
        
        public delegate bool TryGetTaskSchedulerCallback(string name, out TaskScheduler taskScheduler);

        void ProcessWithTaskScheduler(AccessWithReturn callback, IRemoteAgencyMessage message, out IRemoteAgencyMessage response, out Exception exception, out LocalExceptionHandlingMode localExceptionHandlingMode)
        {
            localExceptionHandlingMode = LocalExceptionHandlingMode.Redirect;
            try
            {
                // ReSharper disable once AsyncConverter.AsyncWait
                var result = _taskFactory
                    .StartNew(() => ProcessWithTaskSchedulerWithResponseInternal(callback, message)).Result;
                response = result.Item1;
                exception = result.Item2;
                localExceptionHandlingMode = result.Item3;
            }
#pragma warning disable CA1031 // Do not catch general exception types
            catch (TaskCanceledException)
            {
                response = default;
                exception = default;
            }
#pragma warning restore CA1031 // Do not catch general exception types
        }

        static Tuple<IRemoteAgencyMessage, Exception, LocalExceptionHandlingMode> ProcessWithTaskSchedulerWithResponseInternal(
            AccessWithReturn callback, IRemoteAgencyMessage message)
        {
            var response = callback(message, out var exception, out var localExceptionHandlingMode);
            return new Tuple<IRemoteAgencyMessage, Exception, LocalExceptionHandlingMode>(response, exception, localExceptionHandlingMode);
        }

        void ProcessWithTaskScheduler(AccessWithoutReturn callback, IRemoteAgencyMessage message, out Exception exception, out LocalExceptionHandlingMode localExceptionHandlingMode)
        {
            localExceptionHandlingMode = LocalExceptionHandlingMode.Redirect;
            try
            {
                // ReSharper disable once AsyncConverter.AsyncWait
                var result = _taskFactory
                    .StartNew(() => ProcessWithTaskSchedulerInternal(callback, message)).Result;
                exception = result.Item1;
                localExceptionHandlingMode = result.Item2;
            }
#pragma warning disable CA1031 // Do not catch general exception types
            catch (TaskCanceledException)
            {
                exception = default;
            }
#pragma warning restore CA1031 // Do not catch general exception types
        }

        static Tuple<Exception, LocalExceptionHandlingMode> ProcessWithTaskSchedulerInternal(AccessWithoutReturn callback, IRemoteAgencyMessage message)
        {
            var localExceptionHandlingMode = LocalExceptionHandlingMode.Redirect;
            try
            {
                callback(message, out localExceptionHandlingMode);
                return new Tuple<Exception, LocalExceptionHandlingMode>(null, localExceptionHandlingMode);
            }
#pragma warning disable CA1031 // Do not catch general exception types
            catch (Exception ex)
            {
                return new Tuple<Exception, LocalExceptionHandlingMode>(ex, localExceptionHandlingMode);
            }
#pragma warning restore CA1031 // Do not catch general exception types
        }

        async void CallCloseRequestedByManagingObjectWithTaskScheduler(AccessCloseRequestedByManagingObject callback, bool sendSpecialCommand)
        {
            try
            {
                await _taskFactory
                    .StartNew(() => callback(sendSpecialCommand)).ConfigureAwait(false);
            }
#pragma warning disable CA1031 // Do not catch general exception types
            catch (TaskCanceledException)
            {
            }
#pragma warning restore CA1031 // Do not catch general exception types
        }

        async void CallOnRemoteClosedWithTaskScheduler(AccessOnRemoteClosed callback, Guid siteId, Guid? instanceId)
        {
            try
            {
                await _taskFactory
                    .StartNew(() => callback(siteId, instanceId)).ConfigureAwait(false);
            }
#pragma warning disable CA1031 // Do not catch general exception types
            catch (TaskCanceledException)
            {
            }
#pragma warning restore CA1031 // Do not catch general exception types
        }

        #endregion

    }
}
