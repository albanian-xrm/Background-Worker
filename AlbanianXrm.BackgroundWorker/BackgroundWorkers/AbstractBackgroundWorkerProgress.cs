using System;
using System.Threading;

namespace AlbanianXrm.BackgroundWorker
{
    internal abstract class AbstractBackgroundWorkerProgress<TValue, TProgress> : AlBackgroundWorker
    {
        public AbstractBackgroundWorkerProgress(SynchronizationContext synchronizationContext) : base(synchronizationContext)
        {
            base.postCallback = new SendOrPostCallback(InternalProgress);
        }

        public Action<int, TProgress> OnProgress { get; set; }

        public Action<TValue, Exception> WorkFinished { get; set; }

        protected void InternalProgress(int percentage, TProgress progress)
        {
            synchronizationContext.Post(postCallback, new BackgroundWorkProgress<TValue, TProgress>(percentage, progress));
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
                else { OnProgress?.Invoke(state.Percentage, state.Progress); }
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
