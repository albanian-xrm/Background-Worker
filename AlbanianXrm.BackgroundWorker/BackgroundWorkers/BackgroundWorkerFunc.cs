using System;
using System.Threading;
using System.Threading.Tasks;

namespace AlbanianXrm.BackgroundWorker
{
    internal class BackgroundWorkerFunc<T, TResult> : AbstractBackgroundWorkerFunc<T, TResult>
    {
        public BackgroundWorkerFunc(SynchronizationContext synchronizationContext) : base(synchronizationContext) { }

        public Func<T, TResult> Work { get; internal set; }

        public T Argument { get; set; }

        internal override void DoWork()
        {
            NotifyOnBeforeStart();
            task = Task.Factory.StartNew(InternalDoWork, CancellationToken.None, TaskCreationOptions.PreferFairness, TaskScheduler.Default);
        }

        private void InternalDoWork()
        {
            TResult result;
            try
            {
                result = Work(Argument);
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
