using System.Collections.ObjectModel;

namespace BackgroundWorker.Services
{
    public class WorkerCollection 
        : Collection<(
            Worker Worker, 
            CancellationTokenSource CancellationTokenSource)>
    {

    }
}
