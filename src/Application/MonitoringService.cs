using System.Threading.Tasks;
using Application.Interfaces;
using Domain.Network;

namespace Application;

public interface IMonitoringService
{
    public void Run();
}

public class MonitoringService : IMonitoringService
{
    private readonly IDeviceExplorer _deviceExplorer;
    private readonly IDeviceReader _deviceReader;
    public List<NetworkInterface> NetworkInterfaces;

    public MonitoringService(IDeviceExplorer deviceExplorer, IDeviceReader deviceReader)
    {
        _deviceExplorer = deviceExplorer;
        _deviceReader = deviceReader;
        NetworkInterfaces = new List<NetworkInterface>();
    }

    public void Run()
    {
        _deviceExplorer.Run();
        NetworkInterfaces = _deviceExplorer.GetNetworkInterfaces();
        
        //Task.Run(_mainLoop);
    }

    private async Task _mainLoop()
    {
        while (true)
        {
            await Task.Delay(1000);
        }
    }
}