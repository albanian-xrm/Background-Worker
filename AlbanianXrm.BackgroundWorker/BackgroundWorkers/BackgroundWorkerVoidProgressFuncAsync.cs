using System;
using System.Threading;
using System.Threading.Tasks;

namespace AlbanianXrm.BackgroundWorker
{
    internal class BackgroundWorkerVoidProgressFuncAsync<T, TProgress> : AbstractBackgroundWorkerVoidProgressFunc<T, TProgress>
    {
        public BackgroundWorkerVoidProgressFuncAsync(SynchronizationContext synchronizationContext) : base(synchronizationContext) { }
        public Func<T, Reporter<TProgress>, Task> Work { get; set; }

        public T Argument { get; set; }

        internal override void DoWork()
        {
            NotifyOnBeforeStart();
            task = Task.Factory.StartNew(InternalDoWork, CancellationToken.None, TaskCreationOptions.PreferFairness, TaskScheduler.Default);
        }

        private async Task InternalDoWork()
        {
            try
            {
                await Work(Argument, new Reporter<TProgress>(InternalProgress));
            }
            catch (Exception e)
            {
                synchronizationContext.Post(postCallback, new BackgroundWorkProgress<T, object, TProgress>(Argument, e));
                return;
            }
            synchronizationContext.Post(postCallback, new BackgroundWorkProgress<T, object, TProgress>(Argument, null));
            return;
        }
    }
}
