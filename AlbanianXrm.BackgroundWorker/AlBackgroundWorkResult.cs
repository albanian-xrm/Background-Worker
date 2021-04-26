using System;

namespace AlbanianXrm.BackgroundWorker
{
    internal class AlBackgroundWorkResult
    {
        public AlBackgroundWorkResult(Exception exception)
        {
            this.Exception = exception;
        }
        public Exception Exception { get; internal set; }
    }

    internal class BackgroundWorkResult<T> : AlBackgroundWorkResult
    {
        public BackgroundWorkResult(Exception exception) : base(exception) { }

        public BackgroundWorkResult(T value) : base(null)
        {
            this.Value = value;
        }
        public T Value { get; internal set; }
    }

    internal class BackgroundWorkResult<T, TValue> : BackgroundWorkResult<TValue>
    {
        public BackgroundWorkResult(T argument, Exception exception) : base(exception) { this.Argument = argument; }

        public BackgroundWorkResult(T argument, TValue value) : base(value)
        {
            this.Argument = argument;
        }
        public T Argument { get; internal set; }
    }
}
