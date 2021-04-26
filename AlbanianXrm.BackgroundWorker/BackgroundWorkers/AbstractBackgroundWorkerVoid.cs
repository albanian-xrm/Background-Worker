using System;
using System.Threading;

namespace AlbanianXrm.BackgroundWorker
{
    internal abstract class AbstractBackgroundWorkerVoid : AlBackgroundWorker
    {
        public Action<Exception> WorkFinished { get; set; }

        public AbstractBackgroundWorkerVoid(SynchronizationContext synchronizationContext) : base(synchronizationContext)
        {
            this.postCallback = new SendOrPostCallback(InternalProgress);
        }

        private void InternalProgress(object stateObject)
        {
            BackgroundWorkBase<object> state = (BackgroundWorkBase<object>)stateObject;
            if (state.Finished)
            {
                try
                {
                    WorkFinished?.Invoke(state.Result.Exception);
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
