using System;
using System.Threading;
using System.Threading.Tasks;

namespace AlbanianXrm.BackgroundWorker
{
    internal class BackgroundWorkerProgress<TValue, TProgress> : AbstractBackgroundWorkerProgress<TValue, TProgress>
    {
        public BackgroundWorkerProgress(SynchronizationContext synchronizationContext) : base(synchronizationContext) { }

        public Func<Reporter<TProgress>, TValue> Work { get; set; }

       public override void DoWork()
        {
            NotifyOnBeforeStart();
            task = Task.Factory.StartNew(InternalDoWork, CancellationToken.None, TaskCreationOptions.PreferFairness, TaskScheduler.Default);
        }

        private void InternalDoWork()
        {
            TValue result;
            try
            {
                result = Work(new Reporter<TProgress>(InternalProgress));
            }
            catch (Exception e)
            {
                synchronizationContext.Post(postCallback, new BackgroundWorkProgress<TValue, TProgress>(e));
                return;
            }
            synchronizationContext.Post(postCallback, new BackgroundWorkProgress<TValue, TProgress>(result));
            return;
        }
    }
}
