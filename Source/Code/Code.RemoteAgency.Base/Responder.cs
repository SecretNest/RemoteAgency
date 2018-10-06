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
    public class Responder<T> // where TEntityBase : class
    {
        ConcurrentDictionary<Guid, ResponderItem> responders = new ConcurrentDictionary<Guid, ResponderItem>();

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
            ResponderItem item;
            if (responders.TryGetValue(messageId, out item))
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
            ResponderItem item;
            if (responders.TryGetValue(messageId, out item))
            {
                item.SetResult(default(T));
            }
        }

        /// <summary>
        /// Sends a exception to the original request.
        /// </summary>
        /// <param name="messageId">Id of the message.</param>
        /// <param name="exception">Exception object.</param>
        /// <remarks>Will raise the exception represented by <paramref name="exception"/> in the calling of <see cref="GetResult(Guid, int)"/>.</remarks>
        public void SetException(Guid messageId, WrappedException exception)
        {
            ResponderItem item;
            if (responders.TryGetValue(messageId, out item))
            {
                item.SetException(exception);
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
            ResponderItem item;
            if (responders.TryGetValue(messageId, out item))
            {
                if (item.GetResult(millisecondsTimeout, out var value))
                {
                    responders.TryRemove(messageId, out _);
                    return value;
                }
                else
                {
                    responders.TryRemove(messageId, out _);
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
        /// <remarks>This should be called before calling <see cref="SetDefaultResult(Guid)"/>, <see cref="SetResult(Guid, T)"/> and <see cref="SetException(Guid, WrappedException)"/>.</remarks>
        public void Prepare(Guid messageId)
        {
            responders.TryAdd(messageId, new ResponderItem());
        }

        /// <summary>
        /// Removes the instance of the matching.
        /// </summary>
        /// <param name="messageId">Id of the message.</param>
        /// <remarks>This method just remove the matching from managing, will not unblock the <see cref="GetResult(Guid, int)"/> calling.</remarks>
        public void Remove(Guid messageId)
        {
            responders.TryRemove(messageId, out _);
        }

        //public void SetExceptionAndRemove(Guid messageId, WrappedException exception)
        //{
        //    ResponderItem item;
        //    if (responders.TryRemove(messageId, out item))
        //    {
        //        item.SetException(exception);
        //    }
        //}

        class ResponderItem
        {
            T value;
            WrappedException exception;

            readonly ManualResetEvent waitHandle = new ManualResetEvent(false);

            public void SetResult(T value)
            {
                this.value = value;
                waitHandle.Set();
            }

            public void SetException(WrappedException exception)
            {
                this.exception = exception;
                waitHandle.Set();
            }

            public bool GetResult(int millisecondsTimeout, out T value)
            {
                if (waitHandle.WaitOne(millisecondsTimeout))
                {
                    if (exception != null)
                    {
                        throw exception.ExceptionGeneric;
                    }
                    else
                    {
                        value = this.value;
                        return true;
                    }
                }
                else
                {
                    value = default(T);
                    return false;
                }
            }
        }
    }
}
