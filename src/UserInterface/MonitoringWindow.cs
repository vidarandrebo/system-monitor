using Domain;
using Monitors;
using Serilog;
using Terminal.Gui;

namespace UserInterface;

public class MonitoringWindow : Window
{
    private List<Task> _tasks;
    private TemperatureMonitor _temperatureMonitor;
    private ILogger _logger;

    public MonitoringWindow()
    {
        _logger = Log.ForContext<Program>();
        _temperatureMonitor = new TemperatureMonitor();
        _tasks = new List<Task>();
        Title = "System Monitor (Ctrl+Q to quit)";
        var valueSomething = new Label()
        {
            Text = "1"
        };
        Add(valueSomething);
        _tasks.Add(Task.Run(() => UpdateValue(valueSomething)));
        _logger.Information("Created monitoring window");
    }

    public async Task UpdateValue(Label label)
    {
        var dev = new DeviceTemperature("/sys/class/hwmon/hwmon2/temp1_input");
        while (true)
        {
            _temperatureMonitor.UpdateDevice(dev);
            label.Text = $"Tctl - Current: {dev.Value}, Min: {dev.Min}, Max: {dev.Max}";
            await Task.Delay(1000);
            Application.MainLoop.Invoke(() => Application.Refresh());
        }
    }
}