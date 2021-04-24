using System;
using System.Threading;
using System.Threading.Tasks;

namespace AlbanianXrm.BackgroundWorker
{
    internal class BackgroundWorkerProgressFuncAsync<T, TValue, TProgress> : AbstractBackgroundWorkerProgressFunc<T, TValue, TProgress>
    {
        public BackgroundWorkerProgressFuncAsync(SynchronizationContext synchronizationContext) : base(synchronizationContext) { }
        public Func<T, Reporter<TProgress>, Task<TValue>> Work { get; set; }

        public T Argument { get; set; }

        internal override void DoWork()
        {
            NotifyOnBeforeStart();
            task = Task.Factory.StartNew(InternalDoWork, CancellationToken.None, TaskCreationOptions.PreferFairness, TaskScheduler.Default);
        }

        private async Task InternalDoWork()
        {
            TValue result;
            try
            {
                result = await Work(Argument, new Reporter<TProgress>(InternalProgress));
            }
            catch (Exception e)
            {
                synchronizationContext.Post(postCallback, new BackgroundWorkProgress<T, TValue, TProgress>(Argument, e));
                return;
            }
            synchronizationContext.Post(postCallback, new BackgroundWorkProgress<T, TValue, TProgress>(Argument, result));
            return;
        }
    }
}
