using Microsoft.Extensions.Logging;
using Terminal.Gui;

namespace UserInterface;

public interface IApplicationWindow
{
    public void Run();
}
public class ApplicationWindow : IApplicationWindow
{
    private readonly ILogger<ApplicationWindow> _logger;
    private readonly MonitoringWindow _monitoringWindow;

    public ApplicationWindow(ILogger<ApplicationWindow> logger, MonitoringWindow monitoringWindow)
    {
        _monitoringWindow = monitoringWindow;
        _logger = logger;
    }
    public void Run() {
        Application.Init();
        Application.Top.Add(_monitoringWindow);
        _logger.LogInformation("Starting user interface");
        Application.Run();
        Application.Shutdown();
        _logger.LogInformation("Shutting down user interface");
    }
}