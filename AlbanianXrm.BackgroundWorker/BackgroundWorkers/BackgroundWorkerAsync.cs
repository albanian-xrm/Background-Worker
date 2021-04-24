using System;
using System.Threading;
using System.Threading.Tasks;

namespace AlbanianXrm.BackgroundWorker
{
    internal class BackgroundWorkerAsync<TResult> : AbstractBackgroundWorker<TResult>
    {
        public BackgroundWorkerAsync(SynchronizationContext synchronizationContext) : base(synchronizationContext) { }

        public Func<Task<TResult>> Work { get; internal set; }

        internal override void DoWork()
        {
            NotifyOnBeforeStart();
            task = Task.Factory.StartNew(InternalDoWork, CancellationToken.None, TaskCreationOptions.PreferFairness, TaskScheduler.Default);
        }

        private async Task InternalDoWork()
        {
            TResult result;
            try
            {
                result = await Work();
            }
            catch (Exception e)
            {
                synchronizationContext.Post(postCallback, new BackgroundWorkBase<TResult>(e));
                return;
            }
            synchronizationContext.Post(postCallback, new BackgroundWorkBase<TResult>(result));
            return;
        }
    }
}
