using System.Threading.Tasks;
using SecretNest.TaskSchedulers;

namespace SecretNest.RemoteAgency
{
    partial class RemoteAgencyManagingObject
    { 
        private SequentialScheduler _sequentialScheduler;

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
