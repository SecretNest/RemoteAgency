using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace SecretNest.RemoteAgency
{
    partial class RemoteAgency
    {
        /// <summary>
        /// Gets or sets whether the in memory type cache for built classes is enabled or not. Default value is true.
        /// </summary>
        /// <remarks>When this cache is enabled, the built classes (proxy and service wrapper) are hold in memory of this Remote Agency instance for avoiding further type building on the same interface and the same purpose.</remarks>
        public bool IsInMemoryTypeCacheEnabled
        {
            get => _inMemoryProxyTypeCache != null;
            set
            {
                if (_inMemoryProxyTypeCache != null)
                {
                    if (!value)
                    {
                        DisableInMemoryTypeCache();
                    }
                }
                else
                {
                    if (value)
                    {
                        EnableInMemoryTypeCache();
                    }
                }
            }
        }

        void EnableInMemoryTypeCache()
        {
            _inMemoryProxyTypeCache = new ConcurrentDictionary<Type, Type>();
            _inMemoryServiceWrapperTypeCache = new ConcurrentDictionary<Type, Type>();
        }

        void DisableInMemoryTypeCache()
        {
            _inMemoryProxyTypeCache?.Clear();
            _inMemoryServiceWrapperTypeCache?.Clear();
            _inMemoryProxyTypeCache = null;
            _inMemoryServiceWrapperTypeCache = null;
        }

        private ConcurrentDictionary<Type, Type> _inMemoryProxyTypeCache;
        private ConcurrentDictionary<Type, Type> _inMemoryServiceWrapperTypeCache;

    }
}
