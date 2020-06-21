using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SecretNest.RemoteAgency.Attributes;

namespace SecretNest.RemoteAgency
{
    partial class RemoteAgencyManagingObject
    {

        #region Entry methods
        protected void ProcessThreadLockWithReturn(
            AccessWithReturn callback, IRemoteAgencyMessage message,
            out IRemoteAgencyMessage response, out Exception exception)
            => _processThreadLockWithReturn(callback, message, out response, out exception);

        protected void ProcessThreadLockWithoutReturn(
            AccessWithoutReturn callback, IRemoteAgencyMessage message, out Exception exception)
            => _processThreadLockWithoutReturn(callback, message, out exception);

        protected void CallClose()
            => _callClose();

        protected void CallOnClosing(AccessOnClosing callback, Guid siteId, Guid? instanceId)
            => _callOnClosing(callback, siteId, instanceId);

        #endregion

        #region Entry methods delegate and maintenance

        private AccessWithReturnCaller _processThreadLockWithReturn;
        private AccessWithoutReturnCaller _processThreadLockWithoutReturn;
        private CallCloseCaller _callClose;
        private CallOnClosingCaller _callOnClosing;

        protected delegate IRemoteAgencyMessage AccessWithReturn(IRemoteAgencyMessage message, out Exception exception);
        protected delegate void AccessWithoutReturn(IRemoteAgencyMessage message);
        protected delegate void AccessOnClosing(Guid siteId, Guid? instanceId);

        delegate void AccessWithReturnCaller(AccessWithReturn callback, IRemoteAgencyMessage message,
            out IRemoteAgencyMessage response, out Exception exception);
        delegate void AccessWithoutReturnCaller(AccessWithoutReturn callback, IRemoteAgencyMessage message,
            out Exception exception);
        delegate void CallCloseCaller();
        delegate void CallOnClosingCaller(AccessOnClosing callback, Guid siteId, Guid? instanceId);

        void InitializeThreadLock(ThreadLockMode threadLockMode)
        {
            switch (threadLockMode)
            {
                case ThreadLockMode.None:
                    _processThreadLockWithReturn = ProcessWithNoThreadLock;
                    _processThreadLockWithoutReturn = ProcessWithNoThreadLock;
                    _callClose = CallCloseWithNoThreadLock;
                    _callOnClosing = CallOnClosingWithNoThreadLock;
                    break;
                case ThreadLockMode.SynchronizationContext:
                    _taskFactory = new TaskFactory(TaskScheduler.FromCurrentSynchronizationContext());
                    _processThreadLockWithReturn = ProcessWithTaskScheduler;
                    _processThreadLockWithoutReturn = ProcessWithTaskScheduler;
                    _callClose = CallCloseWithTaskScheduler;
                    _callOnClosing = CallOnClosingWithTaskScheduler;
                    break;
                case ThreadLockMode.AnyButSameThread:
                    PrepareSequentialScheduler();
                    _processThreadLockWithReturn = ProcessWithTaskScheduler;
                    _processThreadLockWithoutReturn = ProcessWithTaskScheduler;
                    _callClose = CallCloseWithTaskScheduler;
                    _callOnClosing = CallOnClosingWithTaskScheduler;
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
                _callClose = CallCloseWithTaskScheduler;
                _callOnClosing = CallOnClosingWithTaskScheduler;
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
            _callClose = null;
            _callOnClosing = null;

            DisposeSequentialScheduler();
        }


        #endregion

        #region No Lock
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

        void CallCloseWithNoThreadLock()
        {
            CloseManagingObject();
        }

        void CallOnClosingWithNoThreadLock(AccessOnClosing callback, Guid siteId, Guid? instanceId)
        {
            callback(siteId, instanceId);
        }

        #endregion

        #region TaskScheduelr
        private TaskFactory _taskFactory = null;
        
        public delegate bool TryGetTaskSchedulerCallback(string name, out TaskScheduler taskScheduler);

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

        void CallCloseWithTaskScheduler()
        {
            try
            {
                _taskFactory
                    .StartNew(CloseManagingObject).Wait();
            }
            catch (TaskCanceledException)
            {
            }
        }

        void CallOnClosingWithTaskScheduler(AccessOnClosing callback, Guid siteId, Guid? instanceId)
        {
            try
            {
                _taskFactory
                    .StartNew(() => callback(siteId, instanceId)).Wait();
            }
            catch (TaskCanceledException)
            {
            }
        }

        #endregion

    }
}
