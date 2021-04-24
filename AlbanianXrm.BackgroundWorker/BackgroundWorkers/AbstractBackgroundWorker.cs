using System;
using System.Threading;

namespace AlbanianXrm.BackgroundWorker
{
    internal abstract class AbstractBackgroundWorker<TResult> : BackgroundWorker
    {
        public Action<TResult, Exception> WorkFinished { get; set; }

        public AbstractBackgroundWorker(SynchronizationContext synchronizationContext) : base(synchronizationContext)
        {
            this.postCallback = new SendOrPostCallback(InternalProgress);
        }

        private void InternalProgress(object stateObject)
        {
            BackgroundWorkBase<TResult> state = (BackgroundWorkBase<TResult>)stateObject;
            if (state.Finished)
            {
                try
                {
                    WorkFinished?.Invoke(state.Result.Value, state.Result.Exception);
                }
                catch (Exception ex)
                {
                    ThrowUnhandledException(ex);
                }
                NotifyOnAfterEnd();
                task = null;
            }
        }
    }
}
