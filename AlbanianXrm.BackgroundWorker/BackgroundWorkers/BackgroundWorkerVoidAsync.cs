using System;
using System.Threading;
using System.Threading.Tasks;

namespace AlbanianXrm.BackgroundWorker
{
    internal class BackgroundWorkerVoidAsync : AbstractBackgroundWorkerVoid
    {
        public BackgroundWorkerVoidAsync(SynchronizationContext synchronizationContext) : base(synchronizationContext) { }

        public Func<Task> Work { get; internal set; }

        internal override void DoWork()
        {
            NotifyOnBeforeStart();
            task = Task.Factory.StartNew(InternalDoWork, CancellationToken.None, TaskCreationOptions.PreferFairness, TaskScheduler.Default);
        }

        private async Task InternalDoWork()
        {
            try
            {
                await Work();
            }
            catch (Exception e)
            {
                synchronizationContext.Post(postCallback, new BackgroundWorkBase<object>(e));
                return;
            }
            synchronizationContext.Post(postCallback, new BackgroundWorkBase<object>());
            return;
        }
    }
}
