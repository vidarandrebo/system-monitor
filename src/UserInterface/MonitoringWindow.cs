using Domain;
using Microsoft.Extensions.Logging;
using Terminal.Gui;

namespace UserInterface;

public class MonitoringWindow : Window
{
    private List<Task> _tasks;
    private ILogger<MonitoringWindow> _logger;
    private Label _label;
    private DeviceTemperature _dev;

    public MonitoringWindow(ILogger<MonitoringWindow> logger)
    {
        _dev = new DeviceTemperature("/sys/class/hwmon/hwmon2/temp1_input");
        _logger = logger;
        _tasks = new List<Task>();
        Title = "System Monitor (Ctrl+Q to quit)";
        _label = new Label()
        {
            Text = "1"
        };
        Add(_label);
        _logger.LogInformation("Created monitoring window");
    }

    public void UpdateValue(int number)
    {
        _label.Text = $"Tctl - Current: {_dev.Value = number}, Min: {_dev.Min}, Max: {_dev.Max}";
        Application.MainLoop.Invoke(() => Application.Refresh());
    }
}