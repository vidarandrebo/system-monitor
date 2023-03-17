using Monitors;

namespace UserInterface;

public interface IMonitoringService
{
    public void Run();
}

public class MonitoringService : IMonitoringService
{
    private readonly IApplicationWindow _applicationWindow;
    private readonly MonitoringWindow _monitoringWindow;
    private readonly ITemperatureMonitor _temperatureMonitor;

    public MonitoringService(IApplicationWindow applicationWindow, MonitoringWindow monitoringWindow,
        ITemperatureMonitor temperatureMonitor)
    {
        _applicationWindow = applicationWindow;
        _monitoringWindow = monitoringWindow;
        _temperatureMonitor = temperatureMonitor;
    }

    public void Run()
    {
        Task.Run(() => MainLoop());
        _applicationWindow.Run();
    }

    public async Task MainLoop()
    {
        var counter = 1;
        while (true)
        {
            await Task.Delay(1000);
            _monitoringWindow.UpdateValue(counter);
            counter++;
        }
    }
}