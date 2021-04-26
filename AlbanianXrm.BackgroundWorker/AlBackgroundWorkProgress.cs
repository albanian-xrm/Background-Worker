using System;

namespace AlbanianXrm.BackgroundWorker
{
    internal class AlBackgroundWorkProgress<TProgress>
    {
        public bool Finished { get; set; }

        public AlBackgroundWorkProgress() : this(null)
        {
        }

        public AlBackgroundWorkProgress(Exception exception)
        {
            this.Result = new AlBackgroundWorkResult(exception);
            this.Finished = true;
        }

        public AlBackgroundWorkProgress(int percentage, TProgress progress)
        {
            this.Percentage = percentage;
            this.Progress = progress;
            this.Finished = false;
        }
        public AlBackgroundWorkResult Result { get; set; }

        public int Percentage { get; set; }
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

        public BackgroundWorkProgress(int percentage, TProgress progress)
        {
            this.Percentage = percentage;
            this.Progress = progress;
            this.Finished = false;
        }

        public int Percentage { get; set; }

        public TProgress Progress { get; set; }
    }

    internal class BackgroundWorkProgress<T, TValue, TProgress> : BackgroundWorkBase<T, TValue>
    {
     

        public BackgroundWorkProgress(T argument) : this(argument, null)
        {
        }

        public BackgroundWorkProgress(T argument, Exception exception) : base(argument, exception) { }

        public BackgroundWorkProgress(T argument, TValue value) : base(argument, value) { }

        public BackgroundWorkProgress(int percentage, TProgress progress) 
        {
            this.Percentage = percentage;
            this.Progress = progress;
            this.Finished = false;
        }

        public int Percentage { get; set; }

        public TProgress Progress { get; set; }
    }
}
