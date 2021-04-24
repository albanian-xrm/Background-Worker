using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace AlbanianXrm.BackgroundWorker
{
    public class BackgroundWorkHandler
    {
        private readonly Queue<BackgroundWorker> queue;
        private readonly SynchronizationContext synchronizationContext;
        private readonly SendOrPostCallback postCallback;
        private readonly int UIThread;

        public BackgroundWorkHandler()
        {
            this.queue = new Queue<BackgroundWorker>();
            synchronizationContext = SynchronizationContext.Current;
            this.postCallback = new SendOrPostCallback(EnqueueBackgroundWork);
            UIThread = Thread.CurrentThread.ManagedThreadId;
        }

        private void EnqueueBackgroundWork(object work)
        {
            EnqueueBackgroundWork((BackgroundWorker)work);
        }

        public void EnqueueBackgroundWork(BackgroundWorker work)
        {
            if (UIThread != Thread.CurrentThread.ManagedThreadId)
            {
                synchronizationContext.Post(postCallback, work);
                return;
            }
            work.OnAfterEnd += BackgroundWorkEnded;
            if (!queue.Any())
            {
                work.DoWork();
            }
            queue.Enqueue(work);
        }

        private void BackgroundWorkEnded()
        {
            queue.Dequeue();
            if (queue.Any())
            {
                var work = queue.Peek();
                work.DoWork();
            }           
        }
    }
}
