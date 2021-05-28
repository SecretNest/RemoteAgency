using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace SecretNest.RemoteAgency
{
    partial class RemoteAgencyManagingObject
    {
        readonly ConcurrentDictionary<Guid, ResponderItem> _responders = new ();

        public List<Guid> GetWaitingMessageIds() => _responders.Keys.ToList();

        public bool TryGetWaitingMessage(Guid messageId, out IRemoteAgencyMessage sentMessage,
            out DateTime startWaiting)
        {
            if (_responders.TryGetValue(messageId, out var responder))
            {
                sentMessage = responder.SentMessage;
                startWaiting = responder.StartWaiting;
                return true;
            }
            else
            {
                sentMessage = default;
                startWaiting = default;
                return false;
            }
        }

        protected IRemoteAgencyMessage ProcessRequestAndWaitResponse(IRemoteAgencyMessage message,
            Action<IRemoteAgencyMessage> afterPreparedCallback, int millisecondsTimeout)
        {
            if (TryProcessRequestAndWaitResponseWithoutException(message, afterPreparedCallback, millisecondsTimeout,
                out var response))
            {
                if (response.Exception == null) return response;
                return response.IsEmptyMessage ? throw response.Exception : response;
            }
            else
            {
                throw new AccessingTimeOutException(message);
            }
        }

        protected bool TryProcessRequestAndWaitResponseWithoutException(IRemoteAgencyMessage message,
            Action<IRemoteAgencyMessage> afterPreparedCallback, int millisecondsTimeout, out IRemoteAgencyMessage response)
        {
            if (millisecondsTimeout == 0)
                millisecondsTimeout = DefaultTimeOutTime;

            using var responder = new ResponderItem(message);
            _responders[message.MessageId] = responder;
            afterPreparedCallback(message);

            if (responder.GetResult(millisecondsTimeout, out response))
            {
                _responders.TryRemove(message.MessageId, out _);
                return true;
            }
            else
            {
                _responders.TryRemove(message.MessageId, out _);
                return false;
            }
        }

        bool WaitingForRespondersClosed()
        {
            DateTime timeoutTime = DateTime.Now.AddMilliseconds(_getWaitingTimeForDisposingCallback());
            while (!_responders.IsEmpty)
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
            if (!_responders.IsEmpty)
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
            public IRemoteAgencyMessage SentMessage { get; private set; }
            public DateTime StartWaiting { get; private set; }
            private IRemoteAgencyMessage _value;

            public ResponderItem(IRemoteAgencyMessage sentMessage)
            {
                SentMessage = sentMessage;
            }

            readonly ManualResetEvent _waitHandle = new (false);

            public bool WaitOnly(int millisecondsTimeout)
            {
                return _waitHandle?.WaitOne(millisecondsTimeout) != false;
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
                StartWaiting = DateTime.Now;
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
                SentMessage = default;
            }
        }
    }
}
