using System;
using System.Threading;
using System.Threading.Tasks;

namespace AlbanianXrm.BackgroundWorker
{
    internal class BackgroundWorkerVoid : AbstractBackgroundWorkerVoid
    {
        public BackgroundWorkerVoid(SynchronizationContext synchronizationContext) : base(synchronizationContext) { }

        public Action Work { get; internal set; }

        internal override void DoWork()
        {
            NotifyOnBeforeStart();
            task = Task.Factory.StartNew(InternalDoWork, CancellationToken.None, TaskCreationOptions.PreferFairness, TaskScheduler.Default);
        }

        private void InternalDoWork()
        {
            try
            {
                Work();
            }
            catch (Exception e)
            {
                synchronizationContext.Post(postCallback, new BackgroundWorkBase<object>(e));
                return;
            }
            synchronizationContext.Post(postCallback, new BackgroundWorkBase<object>(null));
            return;
        }
    }
}
