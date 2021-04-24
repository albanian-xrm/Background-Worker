using System;
using System.Threading;
using System.Threading.Tasks;

namespace AlbanianXrm.BackgroundWorker
{
    internal class BackgroundWorkerProgressFunc<T, TValue, TProgress> : AbstractBackgroundWorkerProgressFunc<T, TValue, TProgress>
    {
        public BackgroundWorkerProgressFunc(SynchronizationContext synchronizationContext) : base(synchronizationContext) { }

        public Func<T, Reporter<TProgress>, TValue> Work { get; set; }

        public T Argument { get; set; }

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
                result = Work(Argument, new Reporter<TProgress>(InternalProgress));
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
