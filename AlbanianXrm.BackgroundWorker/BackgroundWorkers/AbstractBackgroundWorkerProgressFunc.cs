using System;
using System.Threading;

namespace AlbanianXrm.BackgroundWorker
{
    internal abstract class AbstractBackgroundWorkerProgressFunc<T, TValue, TProgress> : AlBackgroundWorker
    {
        public AbstractBackgroundWorkerProgressFunc(SynchronizationContext synchronizationContext) : base(synchronizationContext)
        {
            base.postCallback = new SendOrPostCallback(InternalProgress);
        }

        public Action<TProgress> OnProgress { get; set; }

        public Action<T, TValue, Exception> WorkFinished { get; set; }

        protected void InternalProgress(int percentage, TProgress progress)
        {
            synchronizationContext.Post(postCallback, new BackgroundWorkProgress<T, TValue, TProgress>(percentage, progress));
        }

        private void InternalProgress(object stateObject)
        {
            BackgroundWorkProgress<T, TValue, TProgress> state = (BackgroundWorkProgress<T, TValue, TProgress>)stateObject;
            try
            {
                if (state.Finished)
                {
                    WorkFinished?.Invoke(state.Result.Argument, state.Result.Value, state.Result.Exception);
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
