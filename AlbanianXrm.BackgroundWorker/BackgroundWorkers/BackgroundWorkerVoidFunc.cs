using System;
using System.Threading;
using System.Threading.Tasks;

namespace AlbanianXrm.BackgroundWorker
{
    internal class BackgroundWorkerVoidFunc<TArgument> : AbstractBackgroundWorkerVoidFunc<TArgument>
    {
        public BackgroundWorkerVoidFunc(SynchronizationContext synchronizationContext) : base(synchronizationContext) { }

        public Action<TArgument> Work { get; internal set; }

        public TArgument Argument { get; set; }

       public override void DoWork()
        {
            NotifyOnBeforeStart();
            task = Task.Factory.StartNew(InternalDoWork, CancellationToken.None, TaskCreationOptions.PreferFairness, TaskScheduler.Default);
        }

        private void InternalDoWork()
        {
            try
            {
                Work(Argument);
            }
            catch (Exception e)
            {
                synchronizationContext.Post(postCallback, new BackgroundWorkBase<TArgument, object>(Argument, e));
                return;
            }
            synchronizationContext.Post(postCallback, new BackgroundWorkBase<TArgument, object>(Argument, null));
            return;
        }
    }
}
