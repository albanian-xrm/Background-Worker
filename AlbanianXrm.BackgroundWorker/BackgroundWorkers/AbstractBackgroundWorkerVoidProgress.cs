using System;
using System.Threading;

namespace AlbanianXrm.BackgroundWorker
{
    internal abstract class AbstractBackgroundWorkerVoidProgress<TProgress> : AlBackgroundWorker
    {
        public AbstractBackgroundWorkerVoidProgress(SynchronizationContext synchronizationContext) : base(synchronizationContext)
        {
            base.postCallback = new SendOrPostCallback(InternalProgress);
        }

        public Action<TProgress> OnProgress { get; set; }

        public Action<Exception> WorkFinished { get; set; }

        protected void InternalProgress(int percentage, TProgress progress)
        {
            synchronizationContext.Post(postCallback, new AlBackgroundWorkProgress<TProgress>(percentage, progress));
        }

        private void InternalProgress(object stateObject)
        {
            AlBackgroundWorkProgress<TProgress> state = (AlBackgroundWorkProgress<TProgress>)stateObject;
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
