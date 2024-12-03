namespace BackgroundWorker.Services
{
    // ivelin.georgiev.bs@gmail.com
    public class WorkerService
    {
        private readonly JobCollection _jobs;
        private readonly WorkerCollection _workers;

        public WorkerService(
            JobCollection jobs,
            WorkerCollection workers) 
        {
            _jobs = jobs;
            _workers = workers;
        }
        
        public IEnumerable<Worker> GetWorkers()
        {
            return _workers
                .Select(e => e.Worker)
                .ToArray();
        }

        public Worker CreateWorker(string name)
        {
            var cancellationTokenSource = new CancellationTokenSource();

            var worker = new Worker(
                id: Guid.NewGuid().ToString(),
                name: name,
                cancellationToken: cancellationTokenSource.Token);

            _workers.Add((worker, cancellationTokenSource));

            worker.WorkerThread = Task.Run(async () =>
            {
               
                while(!worker.CancellationToken.IsCancellationRequested)
                {
                    if (!GlobalLock.ResetEvent.WaitOne(100))
                    {
                        Console.WriteLine($"Worker [{worker.Id}] paused...");
                    };

                    GlobalLock.ResetEvent.WaitOne();
                    Console.WriteLine($"Worker [{worker.Id}] waiting for jobs...");

                    if (!worker.IsWorking)
                    {
                        //await GlobalLock.Semaphore.WaitAsync();
                        lock (GlobalLock.Lock)
                        {
                            var job = _jobs
                                .Where(e => e.Status == Status.Queued)
                                .FirstOrDefault();

                            if (job != null)
                            {
                                worker.AssignJob(job);
                            }
                        }
                        //GlobalLock.Semaphore.Release();
                        await worker.WaitForJobAsync();
                        
                        if (worker.IsWorking)
                        {
                            worker.CompleteJob();
                        }
                    }

                    await Task.Delay(3000);
                }

                Console.WriteLine($"Worker with id {worker.Id} has stopped");

            }, worker.CancellationToken);

            return worker;
        }

        public Worker DeleteWorker(string workerId)
        {
            var worker = _workers
                .Where(e => e.Worker.Id == workerId)
                .FirstOrDefault();

            if (worker.Worker is null)
            {
                throw new InvalidOperationException(
                    $"Worker with id {workerId} was not found");
            }

            worker.CancellationTokenSource.Cancel();

            _workers.Remove(worker);

            return worker.Worker;
        }
    }
}
