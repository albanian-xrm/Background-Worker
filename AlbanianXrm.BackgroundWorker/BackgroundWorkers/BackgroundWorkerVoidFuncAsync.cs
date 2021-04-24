using System;
using System.Threading;
using System.Threading.Tasks;

namespace AlbanianXrm.BackgroundWorker
{
    internal class BackgroundWorkerVoidFuncAsync<T> : AbstractBackgroundWorkerVoidFunc<T>
    {
        public BackgroundWorkerVoidFuncAsync(SynchronizationContext synchronizationContext) : base(synchronizationContext) { }

        public Func<T, Task> Work { get; set; }

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
                await Work(Argument);
            }
            catch (Exception e)
            {
                synchronizationContext.Post(postCallback, new BackgroundWorkBase<T, object>(Argument, e));
                return;
            }
            synchronizationContext.Post(postCallback, new BackgroundWorkBase<T, object>(Argument, null));
            return;
        }
    }
}
