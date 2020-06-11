using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace SecretNest.RemoteAgency
{
    /// <summary>
    /// Manages the matching of the request and response message.
    /// </summary>
    /// <typeparam name="T">Entity base</typeparam>
    public class Responder<T> : IDisposable // where TEntityBase : class
        where T : IRemoteAgencyMessage
    {
        readonly ConcurrentDictionary<Guid, ResponderItem> _responders = new ConcurrentDictionary<Guid, ResponderItem>();

        /// <summary>
        /// Should be called while the waiting of response is timed out.
        /// </summary>
        public MessageWaitingTimedOutCallback MessageWaitingTimedOutCallback { get; set; }

        /// <summary>
        /// Sends the responded value to the original request.
        /// </summary>
        /// <param name="messageId">Id of the message.</param>
        /// <param name="value">Responded value.</param>
        /// <remarks>Will unblock the calling of <see cref="GetResult(Guid, int)"/>.</remarks>
        public void SetResult(Guid messageId, T value)
        {
            if (_responders.TryGetValue(messageId, out var item))
            {
                item.SetResult(value);
            }
        }

        /// <summary>
        /// Sends a default result to the original request.
        /// </summary>
        /// <param name="messageId">Id of the message.</param>
        /// <remarks>Will unblock the calling of <see cref="GetResult(Guid, int)"/>.</remarks>
        public void SetDefaultResult(Guid messageId)
        {
            if (_responders.TryGetValue(messageId, out var item))
            {
                item.SetResult(default(T));
            }
        }

        /// <summary>
        /// Gets the response of the request.
        /// </summary>
        /// <param name="messageId">Id of the message</param>
        /// <param name="millisecondsTimeout">The length of time for waiting response, in milliseconds, or the value -1 to indicate that the waiting does not time out.</param>
        /// <returns>Responded value.</returns>
        /// <exception cref="TimeoutException">This is thrown when waiting is timed out.</exception>
        /// <exception cref="ArgumentOutOfRangeException">This is thrown when message specified by <paramref name="messageId"/> cannot be found.</exception>
        /// <remarks>The instance of the mapping will be removed after this calling end.</remarks>
        public T GetResult(Guid messageId, int millisecondsTimeout)
        {
            if (_responders.TryGetValue(messageId, out var item))
            {
                if (item.GetResult(millisecondsTimeout, out var value))
                {
                    _responders.TryRemove(messageId, out var removed);
                    removed.RemoveItem();
                    return value;
                }
                else
                {
                    _responders.TryRemove(messageId, out var removed);
                    removed.RemoveItem();
                    MessageWaitingTimedOutCallback(messageId);
                    throw new TimeoutException();
                }
            }
            else
            {
                throw new ArgumentOutOfRangeException(nameof(messageId));
            }
        }

        /// <summary>
        /// Prepares for receiving response.
        /// </summary>
        /// <param name="messageId">Id of the message.</param>
        /// <remarks>This should be called before calling <see cref="SetDefaultResult(Guid)"/> and <see cref="SetResult(Guid, T)"/>.</remarks>
        public void Prepare(Guid messageId)
        {
            _responders.TryAdd(messageId, new ResponderItem());
        }

        /// <summary>
        /// Removes the instance of the matching.
        /// </summary>
        /// <param name="messageId">Id of the message.</param>
        /// <remarks>This method just remove the matching from managing, will not unblock the <see cref="GetResult(Guid, int)"/> calling.</remarks>
        public void Remove(Guid messageId)
        {
            _responders.TryRemove(messageId, out var removed);
            removed.RemoveItem();
        }

        class ResponderItem
        {
            private T _value;

            ManualResetEvent _waitHandle = new ManualResetEvent(false);

            public void SetResult(T value)
            {
                _value = value;
                _waitHandle?.Set();
            }

            public bool GetResult(int millisecondsTimeout, out T value)
            {
                try
                {
                    if (_waitHandle.WaitOne(millisecondsTimeout))
                    {
                        value = _value;
                        return true;
                    }
                    else
                    {
                        value = default(T);
                        return false;
                    }
                }
                finally
                {
                    _waitHandle = null;
                    var copy = _waitHandle;
                    copy?.Dispose();
                }
            }

            public void RemoveItem() //will not unblock
            {
                if (_waitHandle == null) return;
                _waitHandle = null;
                var copy = _waitHandle;
                copy?.Dispose();
            }
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        /// <summary>
        /// Disposes of the resources (other than memory) used by this instance.
        /// </summary>
        /// <param name="disposing">True: release both managed and unmanaged resources; False: release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    foreach (var item in _responders)
                    {
                        item.Value.RemoveItem();
                    }
                    _responders.Clear();
                }

                disposedValue = true;
            }
        }

        /// <summary>
        /// Releases all resources used by this instance.
        /// </summary>
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
        }
        #endregion
    }
}
