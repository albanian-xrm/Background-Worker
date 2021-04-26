using System;
using System.Threading;

namespace AlbanianXrm.BackgroundWorker
{
    internal abstract class AbstractBackgroundWorkerVoidFunc<TArgument> : AlBackgroundWorker
    {
        public Action<TArgument, Exception> WorkFinished { get; set; }

        public AbstractBackgroundWorkerVoidFunc(SynchronizationContext synchronizationContext) : base(synchronizationContext)
        {
            this.postCallback = new SendOrPostCallback(InternalProgress);
        }

        private void InternalProgress(object stateObject)
        {
            BackgroundWorkBase<TArgument, object> state = (BackgroundWorkBase<TArgument, object>)stateObject;
            if (state.Finished)
            {
                try
                {
                    WorkFinished?.Invoke(state.Result.Argument, state.Result.Exception);
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
