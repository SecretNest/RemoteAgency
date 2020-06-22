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
            out IRemoteAgencyMessage response, out Exception exception,
            out LocalExceptionHandlingMode localExceptionHandlingMode)
            => _processThreadLockWithReturn(callback, message, out response, out exception,
                out localExceptionHandlingMode);

        protected void ProcessThreadLockWithoutReturn(
            AccessWithoutReturn callback, IRemoteAgencyMessage message, out Exception exception,
            out LocalExceptionHandlingMode localExceptionHandlingMode)
            => _processThreadLockWithoutReturn(callback, message, out exception,
                out localExceptionHandlingMode);

        protected void CallSimpleMethod(Action callback)
            => _callCallSimpleMethod(callback);

        protected void CallOnClosing(AccessOnClosing callback, Guid siteId, Guid? instanceId)
            => _callOnClosing(callback, siteId, instanceId);

        #endregion

        #region Entry methods delegate and maintenance

        private AccessWithReturnCaller _processThreadLockWithReturn;
        private AccessWithoutReturnCaller _processThreadLockWithoutReturn;
        private CallCallSimpleMethodCaller _callCallSimpleMethod;
        private CallOnClosingCaller _callOnClosing;

        protected delegate IRemoteAgencyMessage AccessWithReturn(IRemoteAgencyMessage message, out Exception exception,
            out LocalExceptionHandlingMode localExceptionHandlingMode);
        protected delegate void AccessWithoutReturn(IRemoteAgencyMessage message,
            out LocalExceptionHandlingMode localExceptionHandlingMode);
        protected delegate void AccessOnClosing(Guid siteId, Guid? instanceId);

        delegate void AccessWithReturnCaller(AccessWithReturn callback, IRemoteAgencyMessage message,
            out IRemoteAgencyMessage response, out Exception exception,
            out LocalExceptionHandlingMode localExceptionHandlingMode);
        delegate void AccessWithoutReturnCaller(AccessWithoutReturn callback, IRemoteAgencyMessage message,
            out Exception exception, out LocalExceptionHandlingMode localExceptionHandlingMode);
        delegate void CallCallSimpleMethodCaller(Action callback);
        delegate void CallOnClosingCaller(AccessOnClosing callback, Guid siteId, Guid? instanceId);

        void InitializeThreadLock(ThreadLockMode threadLockMode)
        {
            switch (threadLockMode)
            {
                case ThreadLockMode.None:
                    _processThreadLockWithReturn = ProcessWithNoThreadLock;
                    _processThreadLockWithoutReturn = ProcessWithNoThreadLock;
                    _callCallSimpleMethod = CallSimpleMethodWithNoThreadLock;
                    _callOnClosing = CallOnClosingWithNoThreadLock;
                    break;
                case ThreadLockMode.SynchronizationContext:
                    _taskFactory = new TaskFactory(TaskScheduler.FromCurrentSynchronizationContext());
                    _processThreadLockWithReturn = ProcessWithTaskScheduler;
                    _processThreadLockWithoutReturn = ProcessWithTaskScheduler;
                    _callCallSimpleMethod = CallSimpleMethodWithTaskScheduler;
                    _callOnClosing = CallOnClosingWithTaskScheduler;
                    break;
                case ThreadLockMode.AnyButSameThread:
                    PrepareSequentialScheduler();
                    _processThreadLockWithReturn = ProcessWithTaskScheduler;
                    _processThreadLockWithoutReturn = ProcessWithTaskScheduler;
                    _callCallSimpleMethod = CallSimpleMethodWithTaskScheduler;
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
                _callCallSimpleMethod = CallSimpleMethodWithTaskScheduler;
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
            _callCallSimpleMethod = null;
            _callOnClosing = null;

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
            catch (Exception ex)
            {
                exception = ex;
            }
        }

        void CallSimpleMethodWithNoThreadLock(Action callback)
        {
            callback();
        }

        void CallOnClosingWithNoThreadLock(AccessOnClosing callback, Guid siteId, Guid? instanceId)
        {
            callback(siteId, instanceId);
        }

        #endregion

        #region TaskScheduelr
        private TaskFactory _taskFactory = null;
        
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
            catch (TaskCanceledException)
            {
                response = default;
                exception = default;
            }
        }

        Tuple<IRemoteAgencyMessage, Exception, LocalExceptionHandlingMode> ProcessWithTaskSchedulerWithResponseInternal(
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
            catch (TaskCanceledException)
            {
                exception = default;
            }
        }

        Tuple<Exception, LocalExceptionHandlingMode> ProcessWithTaskSchedulerInternal(AccessWithoutReturn callback, IRemoteAgencyMessage message)
        {
            LocalExceptionHandlingMode localExceptionHandlingMode = LocalExceptionHandlingMode.Redirect;
            try
            {
                callback(message, out localExceptionHandlingMode);
                return new Tuple<Exception, LocalExceptionHandlingMode>(null, localExceptionHandlingMode);
            }
            catch (Exception ex)
            {
                return new Tuple<Exception, LocalExceptionHandlingMode>(ex, localExceptionHandlingMode);
            }
        }

        async void CallSimpleMethodWithTaskScheduler(Action callback)
        {
            try
            {
                await _taskFactory
                    .StartNew(callback).ConfigureAwait(false);
            }
            catch (TaskCanceledException)
            {
            }
        }

        async void CallOnClosingWithTaskScheduler(AccessOnClosing callback, Guid siteId, Guid? instanceId)
        {
            try
            {
                await _taskFactory
                    .StartNew(() => callback(siteId, instanceId)).ConfigureAwait(false);
            }
            catch (TaskCanceledException)
            {
            }
        }

        #endregion

    }
}
