using System;
using System.Reflection.Emit;
using SecretNest.RemoteAgency.Inspecting;

namespace SecretNest.RemoteAgency
{
    partial class RemoteAgencyBase
    {
#pragma warning disable IDE0079 // Remove unnecessary suppression
#pragma warning disable IDE0060 // Remove unused parameter
        static void EmitServiceWrapper(
            //ReSharper disable once UnusedParameter.Local
            TypeBuilder typeBuilder, 
            //ReSharper disable once UnusedParameter.Local
            RemoteAgencyInterfaceInfo info)
#pragma warning restore IDE0060 // Remove unused parameter
#pragma warning restore IDE0079 // Remove unnecessary suppression
        {
            //TODO: write code here to build service wrapper.

            throw new Exception();
        }
    }
}
