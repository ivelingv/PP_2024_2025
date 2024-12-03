using BackgroundWorker.Models;

namespace BackgroundWorker.Services
{
    public static class GlobalLock
    {
        public static object Lock = new object();
        public static SemaphoreSlim Semaphore = new SemaphoreSlim(1, 1);
        public static ManualResetEvent ResetEvent = new ManualResetEvent(false);
    }

    public class JobSevice
    {
        private readonly JobCollection _jobs;

        public JobSevice(JobCollection jobs)
        {
            _jobs = jobs;
        }

        public IEnumerable<Job> GetJobs(
            string? jobName, 
            Priority? priority, 
            Status? status)
        {
            IEnumerable<Job> query = _jobs;

            if (jobName != null) 
            {
                query = query
                    .Where(e => e.Name == jobName);
            }

            if (priority != null)
            {
                query = query
                    .Where(e => e.Priority == priority);
            }

            if (status != null)
            {
                query = query
                    .Where(e => e.Status == status);
            }

            return query.ToArray();
        }

        public Job? GetJob(string jobId)
        {
            return _jobs
                .Where(e => e.Id == jobId)
                .FirstOrDefault();
        }

        public Job CreateJob(
            string? name, 
            string? description, 
            int? timeNeededInSeconds,
            Priority? priority)
        {
            var job = new Job
            {
                Name = name ?? Guid.NewGuid().ToString(),
                Priority = priority ?? Priority.Mid,
                TimeNeededInSeconds = timeNeededInSeconds ?? 5,
                Description = description ?? Guid.NewGuid().ToString(),
                Id = Guid.NewGuid().ToString(),
                Status = Status.Queued,
            };

            _jobs.Add(job);

            return job;
        }

        public Job DeleteJob(string jobId)
        {
            //GlobalLock.Semaphore.Wait();

            lock (GlobalLock.Lock)
            {
                var job = _jobs
                    .Where(e => e.Id == jobId)
                    .Where(e => e.Status != Status.InProgress)
                    .FirstOrDefault();

                if (job is null)
                {
                    throw new InvalidOperationException(
                        "Job is currently executing or it does not exist");
                }

                return job;
            }
        }
    }

    public class Job
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int TimeNeededInSeconds { get; set; }
        public Priority Priority { get; set; } = Priority.Low;
        public Status Status { get; set; } = Status.Queued;
    }
}
