using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SecretNest.RemoteAgency
{
    public partial class RemoteAgencyManager<TNetworkMessage, TSerialized, TEntityBase> where TEntityBase : class
    {
        /// <summary>
        /// Gets or sets the site id of default target.
        /// </summary>
        /// <seealso cref="AddTargetSite(Guid, Guid)"/>
        /// <seealso cref="RemoveTargetSite(Guid)"/>
        /// <seealso cref="ResetTargetSite"/>
        /// <seealso cref="TargetSiteNotFoundException"/>
        /// <seealso cref="GetTargetSiteByInstanceId(Guid)"/>
        /// <seealso cref="GetInstanceIdBySite(Guid)"/>
        /// <seealso cref="GetAllInstancesGroupedBySiteId"/>
        /// <seealso cref="GetAllTargetSites"/>
        /// <remarks>Target site table is a mapping table which records the mapping of the instance and the site where it's located.</remarks>
        public Guid DefaultTargetSiteId { get; set; }
        
        ConcurrentDictionary<Guid, Guid> targetSites = new ConcurrentDictionary<Guid, Guid>(); //instance, site

        /// <summary>
        /// Adds a target site to target site table.
        /// </summary>
        /// <param name="instanceId">Instance id of proxy or service wrapper.</param>
        /// <param name="siteId">Site id.</param>
        /// <seealso cref="RemoveTargetSite(Guid)"/>
        /// <seealso cref="ResetTargetSite"/>
        /// <seealso cref="DefaultTargetSiteId"/>
        /// <seealso cref="TargetSiteNotFoundException"/>
        /// <seealso cref="GetTargetSiteByInstanceId(Guid)"/>
        /// <seealso cref="GetInstanceIdBySite(Guid)"/>
        /// <seealso cref="GetAllInstancesGroupedBySiteId"/>
        /// <seealso cref="GetAllTargetSites"/>
        /// <remarks>Target site table is a mapping table which records the mapping of the instance and the site where it's located.</remarks>
        public void AddTargetSite(Guid instanceId, Guid siteId)
        {
            targetSites.AddOrUpdate(instanceId, siteId, (i,j) => siteId);
        }

        /// <summary>
        /// Removes a target site from target site table.
        /// </summary>
        /// <param name="instanceId">Instance id of proxy or service wrapper.</param>
        /// <seealso cref="AddTargetSite(Guid, Guid)"/>
        /// <seealso cref="ResetTargetSite"/>
        /// <seealso cref="DefaultTargetSiteId"/>
        /// <seealso cref="TargetSiteNotFoundException"/>
        /// <seealso cref="GetTargetSiteByInstanceId(Guid)"/>
        /// <seealso cref="GetInstanceIdBySite(Guid)"/>
        /// <seealso cref="GetAllInstancesGroupedBySiteId"/>
        /// <seealso cref="GetAllTargetSites"/>
        /// <remarks>Target site table is a mapping table which records the mapping of the instance and the site where it's located.</remarks>
        public void RemoveTargetSite(Guid instanceId)
        {
            targetSites.TryRemove(instanceId, out _);
        }

        /// <summary>
        /// Removes all records in target site table.
        /// </summary>
        /// <seealso cref="AddTargetSite(Guid, Guid)"/>
        /// <seealso cref="RemoveTargetSite(Guid)"/>
        /// <seealso cref="DefaultTargetSiteId"/>
        /// <seealso cref="TargetSiteNotFoundException"/>
        /// <seealso cref="GetTargetSiteByInstanceId(Guid)"/>
        /// <seealso cref="GetInstanceIdBySite(Guid)"/>
        /// <seealso cref="GetAllInstancesGroupedBySiteId"/>
        /// <seealso cref="GetAllTargetSites"/>
        /// <remarks>Target site table is a mapping table which records the mapping of the instance and the site where it's located.</remarks>
        public void ResetTargetSite()
        {
            targetSites.Clear();
            DefaultTargetSiteId = Guid.Empty;
        }

        /// <summary>
        /// Queries the target site by instance id specified.
        /// </summary>
        /// <param name="instanceId">Instance id.</param>
        /// <returns>Target site which contains this instance specified. <see cref="Guid.Empty"/> will be returned if target site cannot be found.</returns>
        /// <seealso cref="AddTargetSite(Guid, Guid)"/>
        /// <seealso cref="RemoveTargetSite(Guid)"/>
        /// <seealso cref="ResetTargetSite"/>
        /// <seealso cref="DefaultTargetSiteId"/>
        /// <seealso cref="TargetSiteNotFoundException"/>
        /// <seealso cref="GetInstanceIdBySite(Guid)"/>
        /// <seealso cref="GetAllInstancesGroupedBySiteId"/>
        /// <seealso cref="GetAllTargetSites"/>
        /// <remarks>Target site table is a mapping table which records the mapping of the instance and the site where it's located.</remarks>
        public Guid GetTargetSiteByInstanceId(Guid instanceId)
        {
            if (!targetSites.TryGetValue(instanceId, out Guid result))
            {
                if (DefaultTargetSiteId != Guid.Empty)
                    return DefaultTargetSiteId;
                else
                    return Guid.Empty;
            }
            return result;
        }
        Guid QueryTargetSite(Guid instanceId)
        {
            if (!targetSites.TryGetValue(instanceId, out Guid result))
            {
                if (DefaultTargetSiteId != Guid.Empty)
                    return DefaultTargetSiteId;
                else
                    throw new TargetSiteNotFoundException(TargetSiteNotFoundIdType.InstanceId, instanceId);
            }
            return result;
        }
        Guid QueryDefaultTargetSite() => DefaultTargetSiteId;

        /// <summary>
        /// Gets all instance id by site id specified.
        /// </summary>
        /// <param name="siteId">Site id</param>
        /// <returns>ll instance id recorded with this site.</returns>
        /// <seealso cref="AddTargetSite(Guid, Guid)"/>
        /// <seealso cref="RemoveTargetSite(Guid)"/>
        /// <seealso cref="ResetTargetSite"/>
        /// <seealso cref="DefaultTargetSiteId"/>
        /// <seealso cref="TargetSiteNotFoundException"/>
        /// <seealso cref="GetTargetSiteByInstanceId(Guid)"/>
        /// <seealso cref="GetAllInstancesGroupedBySiteId"/>
        /// <seealso cref="GetAllTargetSites"/>
        /// <remarks>Target site table is a mapping table which records the mapping of the instance and the site where it's located.</remarks>
        public IEnumerable<Guid> GetInstanceIdBySite(Guid siteId)
        {
            return targetSites.Where(i => i.Value == siteId).Select(i => i.Key);
        }

        /// <summary>
        /// Get all instance id grouped by site id.
        /// </summary>
        /// <returns>All instance id grouped by site id.</returns>
        /// <seealso cref="AddTargetSite(Guid, Guid)"/>
        /// <seealso cref="RemoveTargetSite(Guid)"/>
        /// <seealso cref="ResetTargetSite"/>
        /// <seealso cref="DefaultTargetSiteId"/>
        /// <seealso cref="TargetSiteNotFoundException"/>
        /// <seealso cref="GetTargetSiteByInstanceId(Guid)"/>
        /// <seealso cref="GetInstanceIdBySite(Guid)"/>
        /// <seealso cref="GetAllTargetSites"/>
        /// <remarks>Target site table is a mapping table which records the mapping of the instance and the site where it's located.</remarks>
        public IReadOnlyDictionary<Guid, IReadOnlyCollection<Guid>> GetAllInstancesGroupedBySiteId()
        {
            Dictionary<Guid, IReadOnlyCollection<Guid>> result = new Dictionary<Guid, IReadOnlyCollection<Guid>>();
            foreach(var item in targetSites)
            {
                List<Guid> list;
                if (!result.TryGetValue(item.Value, out list))
                {
                    list = new List<Guid>();
                    result.Add(item.Value, list);
                }
                list.Add(item.Key);
            }
            return result;
        }

        /// <summary>
        /// Get all records from target site table.
        /// </summary>
        /// <returns>Full target site table.</returns>
        /// <seealso cref="AddTargetSite(Guid, Guid)"/>
        /// <seealso cref="RemoveTargetSite(Guid)"/>
        /// <seealso cref="ResetTargetSite"/>
        /// <seealso cref="DefaultTargetSiteId"/>
        /// <seealso cref="TargetSiteNotFoundException"/>
        /// <seealso cref="GetTargetSiteByInstanceId(Guid)"/>
        /// <seealso cref="GetInstanceIdBySite(Guid)"/>
        /// <seealso cref="GetAllInstancesGroupedBySiteId"/>
        /// <remarks>Target site table is a mapping table which records the mapping of the instance and the site where it's located.</remarks>
        public IReadOnlyDictionary<Guid, Guid> GetAllTargetSites()
        {
            return targetSites;
        }
    }
}
