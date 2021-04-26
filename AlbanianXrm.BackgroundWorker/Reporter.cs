using System;

namespace AlbanianXrm.BackgroundWorker
{
    public class Reporter<TProgress>
    {
        private readonly Action<int, TProgress> reporter;
        internal Reporter(Action<int, TProgress> reporter)
        {
            this.reporter = reporter;
        }

        public void ReportProgress(int percentage, TProgress progress)
        {
            reporter(percentage, progress);
        }
    }
}
