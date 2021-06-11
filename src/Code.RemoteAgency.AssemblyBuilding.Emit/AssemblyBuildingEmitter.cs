using SecretNest.RemoteAgency.Inspecting;

namespace SecretNest.RemoteAgency
{
    partial class AssemblyBuildingEmitter
    {
        public AssemblyBuildingEmitter(RemoteAgencyInterfaceInfo interfaceInfo)
        {
            InterfaceInfo = interfaceInfo;
        }

        private RemoteAgencyInterfaceInfo InterfaceInfo { get; }
    }
}
