using Domain;
using Serilog;

namespace Monitors;

public class TemperatureMonitor
{
    private readonly ILogger _logger;
    public TemperatureMonitor(ILogger logger)
    {
        MonitoringModules = new List<MonitoringModule>();
        _logger = logger;
        _logger.Information("Created temperaturemonitor");
    }

    public List<MonitoringModule> MonitoringModules { get; set; }

    public async Task Run()
    {
        while (true)
        {
            foreach (var module in MonitoringModules)
            {
                foreach (var device in module.DeviceTemperatures)
                {
                    Task.Run(() => UpdateDevice(device));
                }
            }

            await Task.Delay(1000);
        }
    }

    public List<MonitoringModule> GetMonitoringModules()
    {
        return null;
    }

    public Int64 GetValueFromFile(string fileName)
    {
        try
        {
            using var sr = new StreamReader(fileName);
            return Convert.ToInt64(sr.ReadToEnd());
        }
        catch (IOException)
        {
        }
        catch (Exception ex) when (ex is FormatException or InvalidCastException or OverflowException)
        {
        }

        return 0;
    }

    public void UpdateDevice(DeviceTemperature device)
    {
        var temp = GetValueFromFile(device.FileName);
        device.SetTemp(temp);
    }
}