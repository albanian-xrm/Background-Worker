using System;
using System.Threading;
using System.Threading.Tasks;

namespace AlbanianXrm.BackgroundWorker
{
    internal class BackgroundWorkerFuncAsync<T, TResult> : AbstractBackgroundWorkerFunc<T, TResult>
    {
        public BackgroundWorkerFuncAsync(SynchronizationContext synchronizationContext) : base(synchronizationContext) { }

        public Func<T, Task<TResult>> Work { get; set; }

        public T Argument { get; set; }

       public override void DoWork()
        {
            NotifyOnBeforeStart();
            task = Task.Factory.StartNew(InternalDoWork, CancellationToken.None, TaskCreationOptions.PreferFairness, TaskScheduler.Default);
        }

        private async Task InternalDoWork()
        {
            TResult result;
            try
            {
                result = await Work(Argument);
            }
            catch (Exception e)
            {
                synchronizationContext.Post(postCallback, new BackgroundWorkBase<T, TResult>(Argument, e));
                return;
            }
            synchronizationContext.Post(postCallback, new BackgroundWorkBase<T, TResult>(Argument, result));
            return;
        }
    }
}
