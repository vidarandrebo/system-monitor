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

    public ApplicationWindow()
    {
        _logger = Log.ForContext<Program>();
    }
    public void Run() {
        Application.Init();
        var win = new MonitoringWindow();
        Application.Top.Add(win);
        _logger.Information("Starting user interface");
        Application.Run();
        Application.Shutdown();
        _logger.Information("Shutting down user interface");
    }
}