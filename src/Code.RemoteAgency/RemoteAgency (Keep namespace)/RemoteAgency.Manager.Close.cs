using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using SecretNest.RemoteAgency.Attributes;

namespace SecretNest.RemoteAgency
{
    partial class RemoteAgency
    {
        /// <summary>
        /// Closes all proxy and service wrapper objects.
        /// </summary>
        /// <exception cref="AggregateException">Thrown when exception occurred while disposing instances.</exception>
        public abstract void CloseAllInstances();

        /// <summary>
        /// Closes the proxy or service wrapper by instance id.
        /// </summary>
        /// <param name="instanceId">Instance id of the proxy or service wrapper to be closed.</param>
        /// <returns>Result. <see langword="true"/> when instance is located and closed; <see langword="false"/> when instance is not found.</returns>
        /// <exception cref="AggregateException">Thrown when exception occurred while disposing instance.</exception>
        public abstract bool CloseInstance(Guid instanceId);

        /// <summary>
        /// Closes the proxy object.
        /// </summary>
        /// <param name="proxy">Proxy to be closed.</param>
        /// <returns>Result. <see langword="true"/> when instance is located and closed; <see langword="false"/> when instance is not found.</returns>
        public bool CloseProxy(object proxy)
        {
            var obj = proxy as IProxyCommunicate;
            if (obj == null)
                throw new ArgumentNullException(nameof(proxy), $"Argument {nameof(proxy)} is not set as a proxy object.");

            var instanceId = obj.InstanceId;
            return CloseInstance(instanceId);
        }
    }

    partial class RemoteAgency<TSerialized, TEntityBase>
    {
        /// <inheritdoc />
        public override bool CloseInstance(Guid instanceId)
        {
            if (_managingObjects.TryRemove(instanceId, out var removed))
            {
                removed.Dispose();
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <inheritdoc />
        public override void CloseAllInstances()
        {
            while (_managingObjects.Count > 0)
            {
                var items = _managingObjects.ToArray();

                var tasks = Array.ConvertAll(items, i => Task.Run(() => i.Value.Dispose()));
                foreach (var item in items)
                    _managingObjects.TryRemove(item.Key, out _);

                Task.WaitAll(tasks);
            }
        }
    }
}
