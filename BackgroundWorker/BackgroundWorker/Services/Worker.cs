namespace BackgroundWorker.Services
{
    public class Worker
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public CancellationToken CancellationToken { get; set; }
        public int TotalCompletedJobs { get; set; }
        public Task? WorkerThread { get; set; }

        public Worker(
            string id, 
            string name,
            CancellationToken cancellationToken)
        {
            Id = id;
            Name = name;
            CancellationToken = cancellationToken;
        }
    }
}
