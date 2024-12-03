using BackgroundWorker.Services;

namespace BackgroundWorker.Models
{
    public class JobModel
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
        //public string? Description { get; set; }
        //public int TimeNeededInSeconds { get; set; }
        public JobPriority Priority { get; set; } = JobPriority.Low;
        public JobStatus Status { get; set; } = JobStatus.Queued;
    }

    public static class JobModelExtensions
    {
        public static JobModel? ToModel(this Job? job)
        {
            if (job is null)
            {
                return null;
            }

            return new JobModel
            {
                Id = job.Id,
                Name = job.Name,
                Status = (JobStatus)job.Status,
                Priority = (JobPriority)job.Priority
            };
        }

        public static JobDetailsModel? ToDetailModel(this Job? job)
        {
            if (job is null)
            {
                return null;
            }

            return new JobDetailsModel
            {
                Id = job.Id,
                Name = job.Name,
                Description = job.Description,
                TimeNeededInSeconds = job.TimeNeededInSeconds,
                Status = (JobStatus)job.Status,
                Priority = (JobPriority)job.Priority
            };
        }
    }
}
