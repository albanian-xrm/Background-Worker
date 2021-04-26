using System;
using System.Threading;

namespace AlbanianXrm.BackgroundWorker
{
    internal abstract class AbstractBackgroundWorkerVoidProgressFunc<T, TProgress> : AlBackgroundWorker
    {
        public AbstractBackgroundWorkerVoidProgressFunc(SynchronizationContext synchronizationContext) : base(synchronizationContext)
        {
            base.postCallback = new SendOrPostCallback(InternalProgress);
        }

        public Action<int, TProgress> OnProgress { get; set; }

        public Action<T, Exception> WorkFinished { get; set; }

        protected void InternalProgress(int percentage, TProgress progress)
        {
            synchronizationContext.Post(postCallback, new BackgroundWorkProgress<T, object, TProgress>(percentage, progress));
        }

        private void InternalProgress(object stateObject)
        {
            BackgroundWorkProgress<T, object, TProgress> state = (BackgroundWorkProgress<T, object, TProgress>)stateObject;
            try
            {
                if (state.Finished)
                {
                    WorkFinished?.Invoke(state.Result.Argument, state.Result.Exception);
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
