using System;
using System.Collections.Generic;
using System.Text;
using SecretNest.RemoteAgency.Attributes;

namespace SecretNest.RemoteAgency
{
    partial class RemoteAgencyBase
    {
        private readonly TimeSpan _defaultPingMaxWaitingTime = new TimeSpan(0, 0, 1, 30);

        /// <summary>
        /// Pings to the default target and get the response.
        /// </summary>
        /// <param name="localProxyInstanceId">Instance id of the local proxy which will be used to initiate the ping process.</param>
        /// <param name="maxWaitingTime">Max waiting time.</param>
        /// <param name="remoteSiteId">Will be set to the sender site id of the response message. Or the target site id of the ping message when no response received.</param>
        /// <param name="remoteInstanceId">Will be set to the sender instance id of the response message. Or the target instance id of the ping message when no response received.</param>
        /// <returns>Delay between ping message sending started and response message processing finished.</returns>
        public TimeSpan Ping(Guid localProxyInstanceId, TimeSpan maxWaitingTime, out Guid remoteSiteId, out Guid remoteInstanceId)
        {
            if (TryPing(localProxyInstanceId, maxWaitingTime, out var delay, out remoteSiteId, out remoteInstanceId, out var exception))
            {
                if (exception == null)
                {
                    return delay;
                }
                else
                {
                    throw exception;
                }
            }
            else
            {
                throw exception;
            }
        }

        /// <summary>
        /// Pings to the default target and get the response, using default waiting time which is 90 secs.
        /// </summary>
        /// <param name="localProxyInstanceId">Instance id of the local proxy which will be used to initiate the ping process.</param>
        /// <param name="remoteSiteId">Will be set to the sender site id of the response message. Or the target site id of the ping message when no response received.</param>
        /// <param name="remoteInstanceId">Will be set to the sender instance id of the response message. Or the target instance id of the ping message when no response received.</param>
        /// <returns>Delay between ping message sending started and response message processing finished.</returns>
        public TimeSpan Ping(Guid localProxyInstanceId, out Guid remoteSiteId, out Guid remoteInstanceId)
            => Ping(localProxyInstanceId, _defaultPingMaxWaitingTime, out remoteSiteId, out remoteInstanceId);

        /// <summary>
        /// Tries to ping to the default target and get the response.
        /// </summary>
        /// <param name="localProxyInstanceId">Instance id of the local proxy which will be used to initiate the ping process.</param>
        /// <param name="maxWaitingTime">Max waiting time.</param>
        /// <param name="delay">Will be set to the delay between ping message sending started and response message processing finished.</param>
        /// <param name="remoteSiteId">Will be set to the sender site id of the response message. Or the target site id of the ping message when no response received.</param>
        /// <param name="remoteInstanceId">Will be set to the sender instance id of the response message. Or the target instance id of the ping message when no response received.</param>
        /// <param name="exception">Will be set to the exception object when available.</param>
        /// <returns>Whether the ping request and response processing are finished successfully.</returns>
        public abstract bool TryPing(Guid localProxyInstanceId, TimeSpan maxWaitingTime, out TimeSpan delay,
            out Guid remoteSiteId,
            out Guid remoteInstanceId, out Exception exception);

        /// <summary>
        /// Tries to ping to the default target and get the response, using default waiting time which is 90 secs.
        /// </summary>
        /// <param name="localProxyInstanceId">Instance id of the local proxy which will be used to initiate the ping process.</param>
        /// <param name="delay">Will be set to the delay between ping message sending started and response message processing finished.</param>
        /// <param name="remoteSiteId">Will be set to the sender site id of the response message. Or the target site id of the ping message when no response received.</param>
        /// <param name="remoteInstanceId">Will be set to the sender instance id of the response message. Or the target instance id of the ping message when no response received.</param>
        /// <param name="exception">Will be set to the exception object when available.</param>
        /// <returns>Whether the ping request and response processing are finished successfully.</returns>
        public bool TryPing(Guid localProxyInstanceId, out TimeSpan delay, out Guid remoteSiteId, out Guid remoteInstanceId,
            out Exception exception)
            => TryPing(localProxyInstanceId, _defaultPingMaxWaitingTime, out delay, out remoteSiteId, out remoteInstanceId,
                out exception);
    }

    partial class RemoteAgency<TSerialized, TEntityBase>
    {
        /// <inheritdoc />
        public override bool TryPing(Guid localProxyInstanceId, TimeSpan maxWaitingTime, out TimeSpan delay, out Guid remoteSiteId,
            out Guid remoteInstanceId, out Exception exception)
        {
            if (!_managingObjects.TryGetValue(localProxyInstanceId, out var managingObject))
            {
                throw new ArgumentOutOfRangeException(nameof(localProxyInstanceId), "Proxy specified is not found.");
            }

            var proxy = managingObject as RemoteAgencyManagingObjectProxy<TEntityBase>;
            if (proxy == null)
            {
                throw new ArgumentException("Object specified is not proxy.", nameof(localProxyInstanceId));
            }

            return proxy.TryPing(maxWaitingTime, out delay, out remoteSiteId, out remoteInstanceId, out exception);
        }
    }
}
