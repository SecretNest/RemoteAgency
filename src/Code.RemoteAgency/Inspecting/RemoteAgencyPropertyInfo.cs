using System;
using System.Collections.Generic;
using System.Text;

namespace SecretNest.RemoteAgency.Inspecting
{
    class RemoteAgencyPropertyInfo : RemoteAgencyAssetInfoBase
    {
        public bool IsGettingOneWay { get; set; }
        public bool IsSettingOneWay
        {
            get => IsOneWay;
            set => IsOneWay = value;
        }

        public string GettingRequestEntityName { get; set; }
        public string GettingResponseEntityName { get; set; }
        public List<RemoteAgencyReturnValueInfoBase> GettingResponseEntityProperties { get; set; }

        public string SettingRequestEntityName { get; set; }
        public List<RemoteAgencyParameterInfo> SettingRequestEntityProperties { get; set; }
        public string SettingResponseEntityName { get; set; }
        public List<RemoteAgencyReturnValueInfoBase> SettingResponseEntityProperties { get; set; }

        public int PropertyGettingTimeout { get; set; }
        public int PropertySettingTimeout { get; set; }
    }
}
