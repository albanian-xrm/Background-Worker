using System;

namespace AlbanianXrm.XrmToolBox.Shared
{
    public class Reporter<TProgress>
    {
        private readonly Action<TProgress> reporter;
        internal Reporter(Action<TProgress> reporter)
        {
            this.reporter = reporter;
        }

        public void ReportProgress(TProgress progress)
        {
            reporter(progress);
        }
    }
}
