using Hangfire;

namespace Infrastructure;

public class BackgroundJobs
{
    public void EnqueueJob()
    {
        BackgroundJob.Enqueue(
            () => Console.WriteLine("Fire-and-forget!"));
    }
}