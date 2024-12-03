namespace BackgroundWorker.Services
{
    public class Worker
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public CancellationToken CancellationToken { get; set; }
        public int TotalCompletedJobs { get; set; }
        public Task? WorkerThread { get; set; }
        protected Job? CurrentJob { get; private set; } 

        public Worker(
            string id, 
            string name,
            CancellationToken cancellationToken)
        {
            Id = id;
            Name = name;
            CancellationToken = cancellationToken;
        }

        public void AssignJob(Job job) 
        {
            if (CurrentJob != null)
            {
                throw new InvalidOperationException(
                    "Currently there is a job");
            }

            CurrentJob = job;
            CurrentJob.Status = Status.InProgress;

            Console.WriteLine(
               $"Job with id {CurrentJob.Id} started by worker {Id}");
        }

        public Task WaitForJobAsync()
        {
            if (CurrentJob != null 
                && CurrentJob.TimeNeededInSeconds > 0)
            {
                for (var i = 0; i < CurrentJob.TimeNeededInSeconds; i++)
                {
                    Console.WriteLine(
                        $"Job with id {CurrentJob.Id} in progress...");

                    return Task.Delay(1000);
                }
            }

            return Task.CompletedTask;
        }

        public void CompleteJob()
        {
            if (CurrentJob == null)
            {
                throw new InvalidOperationException(
                    "Currently there is no jobs");
            }

            Console.WriteLine(
                $"Job with id {CurrentJob.Id} completed by worker {Id}");

            CurrentJob.Status = Status.Completed;
            CurrentJob = null;
        }

        public bool IsWorking => CurrentJob != null;
    }
}
