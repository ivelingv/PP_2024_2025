namespace BackgroundWorker.Models
{
    public class JobFilterModel
    {
        public string? JobName { get; set; }
        public JobPriority? Priority { get; set; }
        public JobStatus? Status { get; set; }
    }
}
