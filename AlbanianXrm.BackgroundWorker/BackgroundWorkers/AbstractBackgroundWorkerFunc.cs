using System;
using System.Threading;

namespace AlbanianXrm.BackgroundWorker
{
    internal abstract class AbstractBackgroundWorkerFunc<T, TResult> : AlBackgroundWorker
    {
        public Action<T, TResult, Exception> WorkFinished { get; set; }

        public AbstractBackgroundWorkerFunc(SynchronizationContext synchronizationContext) : base(synchronizationContext)
        {
            base.postCallback = new SendOrPostCallback(InternalProgress);
        }

        private void InternalProgress(object stateObject)
        {
            BackgroundWorkBase<T, TResult> state = (BackgroundWorkBase<T, TResult>)stateObject;
            if (state.Finished)
            {
                try
                {
                    WorkFinished?.Invoke(state.Result.Argument, state.Result.Value, state.Result.Exception);
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
