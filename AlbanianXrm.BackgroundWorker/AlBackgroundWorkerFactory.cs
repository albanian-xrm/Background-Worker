using System;
using System.Threading;
using System.Threading.Tasks;

namespace AlbanianXrm.BackgroundWorker
{
    public static class AlBackgroundWorkerFactory
    {
        private static readonly SynchronizationContext synchronizationContext;

        static AlBackgroundWorkerFactory()
        {
            synchronizationContext = SynchronizationContext.Current;
        }

        public static AlBackgroundWorker NewWorker(Action work, Action<Exception> workFinished = null)
        {
            return new BackgroundWorkerVoid(synchronizationContext)
            {
                Work = work,
                WorkFinished = workFinished
            };
        }
        public static AlBackgroundWorker NewWorker<TResult>(Func<TResult> work, Action<TResult, Exception> workFinished = null)
        {
            return new BackgroundWorker<TResult>(synchronizationContext)
            {
                Work = work,
                WorkFinished = workFinished
            };
        }
        public static AlBackgroundWorker NewWorker<T>(Action<T> work, T argument, Action<T, Exception> workFinished = null)
        {
            return new BackgroundWorkerVoidFunc<T>(synchronizationContext)
            {
                Argument = argument,
                Work = work,
                WorkFinished = workFinished
            };
        }
        public static AlBackgroundWorker NewWorker<T, TResult>(Func<T, TResult> work, T argument, Action<T, TResult, Exception> workFinished = null)
        {
            return new BackgroundWorkerFunc<T, TResult>(synchronizationContext)
            {
                Argument = argument,
                Work = work,
                WorkFinished = workFinished
            };
        }

        public static AlBackgroundWorker NewWorker<TProgress>(Action<Reporter<TProgress>> work, Action<TProgress> progress, Action<Exception> workFinished = null)
        {
            return new BackgroundWorkerVoidProgress<TProgress>(synchronizationContext)
            {
                Work = work,
                OnProgress = progress,
                WorkFinished = workFinished
            };
        }
        public static AlBackgroundWorker NewWorker<TResult, TProgress>(Func<Reporter<TProgress>, TResult> work, Action<TProgress> progress, Action<TResult, Exception> workFinished = null)
        {
            return new BackgroundWorkerProgress<TResult, TProgress>(synchronizationContext)
            {
                Work = work,
                OnProgress = progress,
                WorkFinished = workFinished
            };
        }
        public static AlBackgroundWorker NewWorker<T, TProgress>(Action<T, Reporter<TProgress>> work, T argument, Action<TProgress> progress, Action<T, Exception> workFinished = null)
        {
            return new BackgroundWorkerVoidProgressFunc<T, TProgress>(synchronizationContext)
            {
                Argument = argument,
                Work = work,
                OnProgress = progress,
                WorkFinished = workFinished
            };
        }
        public static AlBackgroundWorker NewWorker<T, TResult, TProgress>(Func<T, Reporter<TProgress>, TResult> work, T argument, Action<TProgress> progress, Action<T, TResult, Exception> workFinished = null)
        {
            return new BackgroundWorkerProgressFunc<T, TResult, TProgress>(synchronizationContext)
            {
                Argument = argument,
                Work = work,
                OnProgress = progress,
                WorkFinished = workFinished
            };
        }

        public static AlBackgroundWorker NewAsyncWorker(Func<Task> work, Action<Exception> workFinished = null)
        {
            return new BackgroundWorkerVoidAsync(synchronizationContext)
            {
                Work = work,
                WorkFinished = workFinished
            };
        }
        public static AlBackgroundWorker NewAsyncWorker<TResult>(Func<Task<TResult>> work, Action<TResult, Exception> workFinished = null)
        {
            return new BackgroundWorkerAsync<TResult>(synchronizationContext)
            {
                Work = work,
                WorkFinished = workFinished
            };
        }
        public static AlBackgroundWorker NewAsyncWorker<T>(Func<T, Task> work, T argument, Action<T, Exception> workFinished = null)
        {
            return new BackgroundWorkerVoidFuncAsync<T>(synchronizationContext)
            {
                Argument = argument,
                Work = work,
                WorkFinished = workFinished
            };
        }
        public static AlBackgroundWorker NewAsyncWorker<T, TResult>(Func<T, Task<TResult>> work, T argument, Action<T, TResult, Exception> workFinished = null)
        {
            return new BackgroundWorkerFuncAsync<T, TResult>(synchronizationContext)
            {
                Argument = argument,
                Work = work,
                WorkFinished = workFinished
            };
        }

        public static AlBackgroundWorker NewAsyncWorker<TProgress>(Func<Reporter<TProgress>, Task> work, Action<TProgress> progress, Action<Exception> workFinished = null)
        {
            return new BackgroundWorkerVoidProgressAsync<TProgress>(synchronizationContext)
            {
                Work = work,
                OnProgress = progress,
                WorkFinished = workFinished
            };
        }
        public static AlBackgroundWorker NewAsyncWorker<TResult, TProgress>(Func<Reporter<TProgress>, Task<TResult>> work, Action<TProgress> progress, Action<TResult, Exception> workFinished = null)
        {
            return new BackgroundWorkerProgressAsync<TResult, TProgress>(synchronizationContext)
            {
                Work = work,
                OnProgress = progress,
                WorkFinished = workFinished
            };
        }
        public static AlBackgroundWorker NewAsyncWorker<T, TProgress>(Func<T, Reporter<TProgress>, Task> work, T argument, Action<TProgress> progress, Action<T, Exception> workFinished = null)
        {
            return new BackgroundWorkerVoidProgressFuncAsync<T, TProgress>(synchronizationContext)
            {
                Argument = argument,
                Work = work,
                OnProgress = progress,
                WorkFinished = workFinished
            };
        }
        public static AlBackgroundWorker NewAsyncWorker<T, TResult, TProgress>(Func<T, Reporter<TProgress>, Task<TResult>> work, T argument, Action<TProgress> progress, Action<T, TResult, Exception> workFinished = null)
        {
            return new BackgroundWorkerProgressFuncAsync<T, TResult, TProgress>(synchronizationContext)
            {
                Argument = argument,
                Work = work,
                OnProgress = progress,
                WorkFinished = workFinished
            };
        }
    }
}
