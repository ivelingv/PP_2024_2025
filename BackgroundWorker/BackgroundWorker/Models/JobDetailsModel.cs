namespace BackgroundWorker.Models
{
    public class JobDetailsModel
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int TimeNeededInSeconds { get; set; }
        public JobPriority Priority { get; set; } = JobPriority.Low;
        public JobStatus Status { get; set; } = JobStatus.Queued;
    }
}
