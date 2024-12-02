using SecretNest.RemoteAgency.Inspecting;

namespace SecretNest.RemoteAgency
{
    internal partial class AssemblyBuildingEmitter
    {
        public AssemblyBuildingEmitter(RemoteAgencyInterfaceInfo interfaceInfo)
        {
            InterfaceInfo = interfaceInfo;
        }

        private RemoteAgencyInterfaceInfo InterfaceInfo { get; }
    }
}
