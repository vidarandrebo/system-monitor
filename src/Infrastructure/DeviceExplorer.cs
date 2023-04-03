using Application;
using Application.Interfaces;
using Domain;
using Domain.Network;
using Domain.Temperature;
using Microsoft.Extensions.Logging;

namespace Infrastructure;

public class DeviceExplorer : IDeviceExplorer
{
    private readonly ILogger<DeviceExplorer> _logger;
    private readonly Dictionary<Guid, NetworkInterface> _networkInterfaces;
    private readonly Dictionary<Guid, TemperatureModule> _temperatureModules;

    public DeviceExplorer(ILogger<DeviceExplorer> logger)
    {
        _logger = logger;
        _networkInterfaces = new Dictionary<Guid, NetworkInterface>();
        _temperatureModules = new Dictionary<Guid, TemperatureModule>();
        _logger.LogInformation("DeviceExplorer Starting");
    }

    public void Run()
    {
        FindNetworkInterfaces();
        FindTemperatureModules();
    }

    public Dictionary<Guid, NetworkInterface> GetNetworkInterfaces()
    {
        return _networkInterfaces;
    }

    public Dictionary<Guid, TemperatureModule> GetTemperatureModules()
    {
        return _temperatureModules;
    }

    private void FindNetworkInterfaces()
    {
        var devices = Directory.GetFileSystemEntries("/sys/class/net");
        foreach (var device in devices)
        {
            var name = device.Split(Path.DirectorySeparatorChar).Last();
            var addressFileName = Path.Join(device, "address");
            var address = FileReader.ReadFileToString(addressFileName);
            if (address.Errors != null)
            {
                _logger.LogError("Failed to read from file {addressFileName}", addressFileName);
                continue;
            }

            var networkInterface = new NetworkInterface(name, address.Value);
            var statisticsFiles = Directory.GetFileSystemEntries(Path.Join(device, "statistics"));
            FindDeviceStatistics(networkInterface, statisticsFiles);
            _networkInterfaces.Add(Guid.NewGuid(), networkInterface);
        }
    }

    private void FindDeviceStatistics(NetworkInterface networkInterface, string[] filePaths)
    {
        foreach (var path in filePaths)
        {
            var fileContent = FileReader.ReadFileToString(path);
            if (fileContent.Errors != null)
            {
                _logger.LogError("Failed to read from file {path}", path);
                continue;
            }

            var name = path.Split(Path.DirectorySeparatorChar).Last();
            var statistic = new Device(name, path);

            networkInterface.AddStatistic(statistic);
        }
    }

    private void FindTemperatureModules()
    {
        var modules = Directory.GetFileSystemEntries("/sys/class/hwmon");
        foreach (var modulePath in modules)
        {
            var name = FileReader.ReadFileToString(Path.Join(modulePath, "name"));
            if (name.Errors != null)
            {
                _logger.LogError("Could not read name from {path}", modulePath);
                continue;
            }

            var module = new TemperatureModule(name.Value);
            var deviceFiles = Directory.GetFileSystemEntries(modulePath);
            FindTemperatureDevices(module, deviceFiles);
            _temperatureModules.Add(Guid.NewGuid(), module);
        }
    }

    private void FindTemperatureDevices(TemperatureModule temperatureModule, string[] filePaths)
    {
        var inputs = FindInputs(filePaths);
        var labels = FindLabels(filePaths);
        foreach (var path in inputs)
        {
            var device = new Device(path.Split(Path.DirectorySeparatorChar).Last().Split('_').First(), path);
            var labelPath = labels.FirstOrDefault(l => l.Contains(device.Name));
            if (!string.IsNullOrEmpty(labelPath))
            {
                var nameResult = FileReader.ReadFileToString(labelPath);
                if (nameResult.Errors == null && nameResult.Value != "")
                {
                    device.Name = nameResult.Value;
                }
            }
            temperatureModule.AddDevice(device);
        }
    }

    private static string[] FindInputs(string[] paths)
    {
        return paths.Where(s => s.Contains("input")).ToArray();
    }

    private static string[] FindLabels(string[] paths)
    {
        return paths.Where(s => s.Contains("label")).ToArray();
    }
}