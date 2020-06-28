using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace SecretNest.RemoteAgency
{
    partial class RemoteAgencyManagingObject
    {
        readonly ConcurrentDictionary<Guid, ResponderItem> _responders = new ConcurrentDictionary<Guid, ResponderItem>();

        protected IRemoteAgencyMessage ProcessRequestAndWaitResponse(IRemoteAgencyMessage message,
            Action<IRemoteAgencyMessage> afterPreparedCallback, int millisecondsTimeout)
        {
            if (millisecondsTimeout == 0)
                millisecondsTimeout = DefaultTimeOutTime;

            using (ResponderItem responder = new ResponderItem())
            {
                _responders[message.MessageId] = responder;
                afterPreparedCallback(message);

                if (responder.GetResult(millisecondsTimeout, out var response))
                {
                    _responders.TryRemove(message.MessageId, out _);
                    if (response.Exception != null)
                    {
                        if (response.IsEmptyMessage)
                        {
                            throw response.Exception;
                        }
                    }
                    return response;
                }
                else
                {
                    _responders.TryRemove(message.MessageId, out _);
                    throw new AccessingTimeOutException(message);
                }
            }
        }

        bool WaitingForRespondersClosed()
        {
            DateTime timeoutTime = DateTime.Now.AddMilliseconds(_getWaitingTimeForDisposingCallback());
            while (_responders.Count > 0)
            {
                var first = _responders.Values.FirstOrDefault();
                if (first == null) continue;

                int timeout = (int)(timeoutTime - DateTime.Now).TotalMilliseconds;
                if (timeout <= 0) return false;

                if (!first.WaitOnly(timeout))
                    return false;
            }

            return true; //all responders are closed gracefully. no need to close forcibly.
        }

        void ForceCloseAllResponders()
        {
            if (_responders.Count > 0)
            {
                var message = CreateEmptyMessage();
                message.Exception = new ObjectDisposedException("RemoteAgency", "Remote Agency instance issues a force closing operation for all communication operations which are still in waiting for response due to disposing.");

                foreach(var value in _responders.Values)
                    value.SetWhenEmpty(message);
            }
        }

        protected void OnResponseReceived(IRemoteAgencyMessage message)
        {
            if (_responders.TryGetValue(message.MessageId, out var responder))
            {
                responder.SetResult(message);
            }
        }

        class ResponderItem : IDisposable
        {
            private IRemoteAgencyMessage _value;

            readonly ManualResetEvent _waitHandle = new ManualResetEvent(false);

            public bool WaitOnly(int millisecondsTimeout)
            {
                return _waitHandle?.WaitOne(millisecondsTimeout) == true;
            }

            public void SetResult(IRemoteAgencyMessage value)
            {
                _value = value;
                _waitHandle?.Set();
            }

            public void SetWhenEmpty(IRemoteAgencyMessage value)
            {
                if (_value == null) _value = value;
                _waitHandle?.Set();
            }

            public bool GetResult(int millisecondsTimeout, out IRemoteAgencyMessage value)
            {
                if (_waitHandle.WaitOne(millisecondsTimeout))
                {
                    value = _value;
                    return true;
                }
                else
                {
                    value = default;
                    return false;
                }
            }

            public void Dispose()
            {
                _waitHandle?.Dispose();
            }
        }
    }
}
