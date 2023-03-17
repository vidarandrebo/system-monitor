namespace UserInterface;

public interface IMonitoringService
{
    public void Run();
}
public class MonitoringService : IMonitoringService
{
    private readonly MonitoringWindow _monitoringWindow;
    private readonly IApplicationWindow _applicationWindow;
    public MonitoringService(IApplicationWindow applicationWindow, MonitoringWindow monitoringWindow)
    {
        _monitoringWindow = monitoringWindow;
        _applicationWindow = applicationWindow;
    }

    public void Run()
    {
        _applicationWindow.Run();
    }
}