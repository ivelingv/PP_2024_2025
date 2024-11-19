namespace BackgroundWorker.Models
{
    public class CreateJobModel
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int TimeNeededInSeconds { get; set; }
        public JobPriority Priority { get; set; } = JobPriority.Low;
    }
}
