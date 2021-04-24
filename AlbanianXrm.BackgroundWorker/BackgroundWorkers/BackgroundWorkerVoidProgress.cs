using System;
using System.Threading;
using System.Threading.Tasks;

namespace AlbanianXrm.BackgroundWorker
{
    internal class BackgroundWorkerVoidProgress<TProgress> : AbstractBackgroundWorkerVoidProgress<TProgress>
    {
        public BackgroundWorkerVoidProgress(SynchronizationContext synchronizationContext) : base(synchronizationContext) { }

        public Action<Reporter<TProgress>> Work { get; set; }

        internal override void DoWork()
        {
            NotifyOnBeforeStart();
            task = Task.Factory.StartNew(InternalDoWork, CancellationToken.None, TaskCreationOptions.PreferFairness, TaskScheduler.Default);
        }

        private void InternalDoWork()
        {
            try
            {
                Work(new Reporter<TProgress>(InternalProgress));
            }
            catch (Exception e)
            {
                synchronizationContext.Post(postCallback, new BackgroundWorkProgress<TProgress>(e));
                return;
            }
            synchronizationContext.Post(postCallback, new BackgroundWorkProgress<TProgress>());
            return;
        }
    }
}
