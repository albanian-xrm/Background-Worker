using System;

namespace AlbanianXrm.BackgroundWorker
{
    internal class BackgroundWorkProgress<TProgress>
    {
        public bool Finished { get; set; }

        public BackgroundWorkProgress() : this(null)
        {
        }

        public BackgroundWorkProgress(Exception exception)
        {
            this.Result = new BackgroundWorkResult(exception);
            this.Finished = true;
        }

        public BackgroundWorkProgress(TProgress progress)
        {
            this.Progress = progress;
            this.Finished = false;
        }
        public BackgroundWorkResult Result { get; set; }


        public TProgress Progress { get; set; }
    }

    internal class BackgroundWorkBase<T>
    {
        public BackgroundWorkBase() : this(null)
        {
        }

        public BackgroundWorkBase(Exception exception)
        {
            this.Result = new BackgroundWorkResult<T>(exception);
            this.Finished = true;
        }

        public BackgroundWorkBase(T value)
        {
            this.Result = new BackgroundWorkResult<T>(value);
            this.Finished = true;
        }

        public bool Finished { get; set; }

        public BackgroundWorkResult<T> Result { get; set; }
    }

    internal class BackgroundWorkBase<T, TValue>
    {
        public BackgroundWorkBase() { }

        public BackgroundWorkBase(T argument) : this(argument, null)
        {
        }

        public BackgroundWorkBase(T argument, Exception exception)
        {
            this.Result = new BackgroundWorkResult<T, TValue>(argument, exception);
            this.Finished = true;
        }

        public BackgroundWorkBase(T argument, TValue value)
        {
            this.Result = new BackgroundWorkResult<T, TValue>(argument, value);
            this.Finished = true;
        }

        public bool Finished { get; set; }

        public BackgroundWorkResult<T, TValue> Result { get; set; }
    }

    internal class BackgroundWorkProgress<T, TProgress> : BackgroundWorkBase<T>
    {
        public BackgroundWorkProgress() : base() { }

        public BackgroundWorkProgress(Exception e) : base(e) { }

        public BackgroundWorkProgress(T value) : base(value) { }

        public BackgroundWorkProgress(TProgress progress)
        {
            this.Progress = progress;
            this.Finished = false;
        }

        public TProgress Progress { get; set; }
    }

    internal class BackgroundWorkProgress<T, TValue, TProgress> : BackgroundWorkBase<T, TValue>
    {
     

        public BackgroundWorkProgress(T argument) : this(argument, null)
        {
        }

        public BackgroundWorkProgress(T argument, Exception exception) : base(argument, exception) { }

        public BackgroundWorkProgress(T argument, TValue value) : base(argument, value) { }

        public BackgroundWorkProgress(TProgress progress)
        {
            this.Progress = progress;
            this.Finished = false;
        }

        public TProgress Progress { get; set; }
    }
}
