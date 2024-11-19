namespace BackgroundWorker.Models
{
    public enum JobStatus
    {
        Queued = 0, 
        InProgress = 1,
        Completed = 2,
        Failed = 99,
    }
}
