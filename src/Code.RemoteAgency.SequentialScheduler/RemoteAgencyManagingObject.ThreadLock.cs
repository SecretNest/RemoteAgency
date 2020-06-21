using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using SecretNest.TaskSchedulers;

namespace SecretNest.RemoteAgency
{
    partial class RemoteAgencyManagingObject
    { 
        private SequentialScheduler _sequentialScheduler = null;

        void PrepareSequentialScheduler()
        {
            _sequentialScheduler = new SequentialScheduler();
            _taskFactory = new TaskFactory(_sequentialScheduler);
        }

        void DisposeSequentialScheduler()
        {
            if (_sequentialScheduler != null)
            {
                _sequentialScheduler.Dispose();
                _sequentialScheduler = null;
            }
        }
    }
}
