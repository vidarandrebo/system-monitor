using Domain;
using Monitors;
using Terminal.Gui;

Application.Run<MonitoringWindow>();

Application.Shutdown();

public class MonitoringWindow : Window
{
    private List<Task> _tasks;
    private TemperatureMonitor _temperatureMonitor;
    public MonitoringWindow()
    {
        _temperatureMonitor = new TemperatureMonitor();
        _tasks = new List<Task>();
        Title = "System Monitor (Ctrl+Q to quit)";
        var valueSomething = new Label()
        {
            Text = "1"
        };
        Add(valueSomething);
        _tasks.Add(Task.Run(() => UpdateValue(valueSomething)));
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
