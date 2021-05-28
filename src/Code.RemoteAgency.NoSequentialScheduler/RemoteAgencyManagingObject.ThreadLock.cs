using System;

namespace SecretNest.RemoteAgency
{
    partial class RemoteAgencyManagingObject
    {
        void PrepareSequentialScheduler()
        {
            throw new NotSupportedException("Neat version of RemoteAgency does not shipped with built-in SequentialScheduler.");
        }

#pragma warning disable IDE0079 // Remove unnecessary suppression
#pragma warning disable CA1822 // Mark members as static
        void DisposeSequentialScheduler()
#pragma warning restore CA1822 // Mark members as static
#pragma warning restore IDE0079 // Remove unnecessary suppression
        {
        }
    }
}
