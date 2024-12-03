namespace BackgroundWorker.Services
{
    public enum Status
    {
        Queued = 0,
        InProgress = 1,
        Completed = 2,
        Failed = 99,
    }
}
