using System;
using System.Threading;
using System.Threading.Tasks;

namespace AlbanianXrm.BackgroundWorker
{
    public abstract partial class BackgroundWorker
    {
        public delegate void LifetimeEvent();
        public event LifetimeEvent OnBeforeStart;
        public event LifetimeEvent OnAfterEnd;

        protected Task task;
        protected readonly SynchronizationContext synchronizationContext;
        protected SendOrPostCallback postCallback;
        public static event EventHandler<Exception> UnhandledException;

        protected BackgroundWorker(SynchronizationContext synchronizationContext)
        {
            this.synchronizationContext = synchronizationContext;
        }

        public abstract void DoWork();

        protected void NotifyOnBeforeStart()
        {
            OnBeforeStart?.Invoke();
        }

        protected void NotifyOnAfterEnd()
        {
            OnAfterEnd?.Invoke();
        }

        protected void ThrowUnhandledException(Exception exception)
        {
            UnhandledException?.Invoke(this, exception);
        }
    }
}
