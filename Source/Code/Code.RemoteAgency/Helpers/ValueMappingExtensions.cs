using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SecretNest.RemoteAgency
{

    static class ValueMappingExtension
    {
        internal static string Add(this List<ValueMapping> mappings, string uniqueName, string typeName, string nameInCode, string preferredPropertyName)
        {
            string propertyName;
            if (string.IsNullOrEmpty(preferredPropertyName))
                propertyName = NamingHelper.MakeFirstUpper(uniqueName);
            else
                propertyName = preferredPropertyName;
            if (mappings.Any(i => i.PropertyName == propertyName))
                propertyName = NamingHelper.GetRandomName(propertyName);
            mappings.Add(new ValueMapping(uniqueName, propertyName, typeName, nameInCode));
            return propertyName;
        }
    }

}
