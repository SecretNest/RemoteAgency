﻿using System;
using System.Collections.Generic;
using System.Text;

namespace SecretNest.RemoteAgency.Inspecting
{
    class RemoteAgencyGenericArgumentInfo
    {
        public Type GenericArgument { get; set; }

        public List<RemoteAgencyAttributePassThrough> PassThroughAttributes { get; set; }
    }
}