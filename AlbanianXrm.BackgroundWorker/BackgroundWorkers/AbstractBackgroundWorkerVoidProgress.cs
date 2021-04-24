using System;
using System.Threading;

namespace AlbanianXrm.BackgroundWorker
{
    internal abstract class AbstractBackgroundWorkerVoidProgress<TProgress> : BackgroundWorker
    {
        public AbstractBackgroundWorkerVoidProgress(SynchronizationContext synchronizationContext) : base(synchronizationContext)
        {
            base.postCallback = new SendOrPostCallback(InternalProgress);
        }

        public Action<TProgress> OnProgress { get; set; }

        public Action<Exception> WorkFinished { get; set; }

        protected void InternalProgress(TProgress progress)
        {
            synchronizationContext.Post(postCallback, new BackgroundWorkProgress<TProgress>(progress));
        }

        private void InternalProgress(object stateObject)
        {
            BackgroundWorkProgress<TProgress> state = (BackgroundWorkProgress<TProgress>)stateObject;
            try
            {
                if (state.Finished)
                {
                    WorkFinished?.Invoke(state.Result.Exception);
                }
                else { OnProgress?.Invoke(state.Progress); }
            }
            catch (Exception ex)
            {
                ThrowUnhandledException(ex);
            }
            if (state.Finished)
            {
                NotifyOnAfterEnd();
                task = null;
            }
        }
    }
}
