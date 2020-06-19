using System;
using System.Collections.Generic;
using System.Text;

namespace SecretNest.RemoteAgency
{
    partial class RemoteAgencyManagingObject<TEntityBase>
    {
        delegate IRemoteAgencyMessage AccessWithReturn(IRemoteAgencyMessage message, out Exception exception);
        delegate void AccessWithoutReturn(IRemoteAgencyMessage message);


    }
}
