using System.Threading.Tasks;

namespace Application;

public interface IMonitoringService
{
    public void Run();
}

public class MonitoringService : IMonitoringService
{

    public void Run()
    {
        Task.Run(_mainLoop);
    }

    private async Task _mainLoop()
    {
        while (true)
        {
            await Task.Delay(1000);
        }
    }
}