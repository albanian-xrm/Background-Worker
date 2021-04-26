using System;
using System.Threading;
using System.Threading.Tasks;

namespace AlbanianXrm.BackgroundWorker
{
    internal class BackgroundWorkerVoidProgressAsync<TProgress> : AbstractBackgroundWorkerVoidProgress<TProgress>
    {
        public BackgroundWorkerVoidProgressAsync(SynchronizationContext synchronizationContext) : base(synchronizationContext) { }

        public Func<Reporter<TProgress>, Task> Work { get; internal set; }

       public override void DoWork()
        {
            NotifyOnBeforeStart();
            task = Task.Factory.StartNew(InternalDoWork, CancellationToken.None, TaskCreationOptions.PreferFairness, TaskScheduler.Default);
        }

        private async Task InternalDoWork()
        {
            try
            {
                await Work(new Reporter<TProgress>(InternalProgress));
            }
            catch (Exception e)
            {
                synchronizationContext.Post(postCallback, new AlBackgroundWorkProgress<TProgress>(e));
                return;
            }
            synchronizationContext.Post(postCallback, new AlBackgroundWorkProgress<TProgress>());
            return;
        }
    }
}
