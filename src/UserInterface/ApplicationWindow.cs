using Serilog;
using Terminal.Gui;

namespace UserInterface;

public interface IApplicationWindow
{
    public void Run();
}
public class ApplicationWindow : IApplicationWindow
{
    private readonly ILogger _logger;
    private readonly MonitoringWindow _monitoringWindow;

    public ApplicationWindow(MonitoringWindow monitoringWindow)
    {
        _monitoringWindow = monitoringWindow;
        _logger = Log.ForContext<Program>();
    }
    public void Run() {
        Application.Init();
        Application.Top.Add(_monitoringWindow);
        _logger.Information("Starting user interface");
        Application.Run();
        Application.Shutdown();
        _logger.Information("Shutting down user interface");
    }
}