namespace BackgroundWorker.Services
{
    // ivelin.georgiev.bs@gmail.com
    public class WorkerService
    {
        private readonly WorkerCollection _workers;

        public WorkerService(
            WorkerCollection workers) 
        {
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
                    // TODO: Check the list of jobs if
                    // there is something to execute.
                    Console.WriteLine($"Worker [{worker.Id}] waiting for jobs...");
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
