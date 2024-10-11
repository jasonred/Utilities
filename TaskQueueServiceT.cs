    public class TaskQueueService<T>
    {
        private readonly int periodDurationMs;
        private readonly int maxRequestsPerPeriod;
        private readonly ConcurrentQueue<Func<Task>> requestQueue = new();
        private readonly Queue<DateTime> requestTimestamps = new();
        private bool isProcessingQueue = false;

        public TaskQueueService(int periodDurationMs, int maxRequestsPerPeriod)
        {
            this.periodDurationMs = periodDurationMs;
            this.maxRequestsPerPeriod = maxRequestsPerPeriod;
        }

        public Task<T> EnqueueRequest(Func<Task<T>> requestFunc)
        {
            var tcs = new TaskCompletionSource<T>();

            requestQueue.Enqueue(async () =>
            {
                try
                {
                    var result = await requestFunc();
                    tcs.SetResult(result);
                }
                catch (Exception ex)
                {
                    tcs.SetException(ex);
                }
            });

            ProcessQueue();

            return tcs.Task;
        }

        private async void ProcessQueue()
        {
            if (isProcessingQueue)
                return;

            isProcessingQueue = true;

            while (requestQueue.TryDequeue(out var request))
            {
                while (requestTimestamps.Count >= maxRequestsPerPeriod &&
                        (DateTime.UtcNow - requestTimestamps.Peek()).TotalMilliseconds < periodDurationMs)
                {
                    await Task.Delay(100); // Adjust delay as necessary
                }

                requestTimestamps.Enqueue(DateTime.UtcNow);
                await request();

                while (requestTimestamps.Count > 0 &&
                        (DateTime.UtcNow - requestTimestamps.Peek()).TotalMilliseconds >= periodDurationMs)
                {
                    requestTimestamps.Dequeue();
                }
            }

            isProcessingQueue = false;
        }
    }
