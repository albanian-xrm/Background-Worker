using System;
using System.Threading;

namespace AlbanianXrm.BackgroundWorker
{
    internal abstract class AbstractBackgroundWorkerProgress<TValue, TProgress> : BackgroundWorker
    {
        public AbstractBackgroundWorkerProgress(SynchronizationContext synchronizationContext) : base(synchronizationContext)
        {
            base.postCallback = new SendOrPostCallback(InternalProgress);
        }

        public Action<TProgress> OnProgress { get; set; }

        public Action<TValue, Exception> WorkFinished { get; set; }

        protected void InternalProgress(TProgress progress)
        {
            synchronizationContext.Post(postCallback, new BackgroundWorkProgress<TValue, TProgress>(progress));
        }

        private void InternalProgress(object stateObject)
        {
            BackgroundWorkProgress<TValue, TProgress> state = (BackgroundWorkProgress<TValue, TProgress>)stateObject;
            try
            {
                if (state.Finished)
                {
                    WorkFinished?.Invoke(state.Result.Value, state.Result.Exception);
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
