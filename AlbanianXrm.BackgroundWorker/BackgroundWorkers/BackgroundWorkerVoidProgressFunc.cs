using System;
using System.Threading;
using System.Threading.Tasks;

namespace AlbanianXrm.BackgroundWorker
{
    internal class BackgroundWorkerVoidProgressFunc<T, TProgress> : AbstractBackgroundWorkerVoidProgressFunc<T, TProgress>
    {
        public BackgroundWorkerVoidProgressFunc(SynchronizationContext synchronizationContext) : base(synchronizationContext) { }

        public Action<T, Reporter<TProgress>> Work { get; set; }

        public T Argument { get; set; }

        internal override void DoWork()
        {
            NotifyOnBeforeStart();
            task = Task.Factory.StartNew(InternalDoWork, CancellationToken.None, TaskCreationOptions.PreferFairness, TaskScheduler.Default);
        }

        private void InternalDoWork()
        {
            try
            {
                Work(Argument, new Reporter<TProgress>(InternalProgress));
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
