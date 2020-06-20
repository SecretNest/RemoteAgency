using System;
using System.Collections.Generic;
using System.Text;

namespace SecretNest.RemoteAgency
{
    partial class RemoteAgencyManagingObject
    {
        void PrepareSequentialScheduler()
        {
            throw new NotSupportedException("Neat version of RemoteAgency does not shipped with built-in SequentialScheduler.");
        }

        void DisposeSequentialScheduler()
        {
        }
    }
}
