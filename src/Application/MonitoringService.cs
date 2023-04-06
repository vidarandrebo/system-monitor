using System.Threading.Channels;
using Application.Interfaces;
using Domain;
using Domain.Network;
using Domain.Temperature;
using Microsoft.Extensions.Logging;

namespace Application;

public class MonitoringService : IMonitoringService
{
    private readonly ILogger<IMonitoringService> _logger;
    private readonly IDeviceExplorer _deviceExplorer;
    private readonly IDeviceReader _deviceReader;
    private ChannelReader<DeviceValue> _valueChannel;
    private Dictionary<Guid, NetworkInterface> _networkInterfaces;
    private Dictionary<Guid, TemperatureModule> _temperatureModules;

    public MonitoringService(IDeviceExplorer deviceExplorer, IDeviceReader deviceReader, ILogger<IMonitoringService> logger)
    {
        _deviceExplorer = deviceExplorer;
        _deviceReader = deviceReader;
        _logger = logger;
        _networkInterfaces = new Dictionary<Guid, NetworkInterface>();
        _temperatureModules = new Dictionary<Guid, TemperatureModule>();
        _valueChannel = _deviceReader.Subscribe();
        _logger.LogInformation("Created instance of MonitoringService");
    }

    public async Task Run()
    {
        _deviceExplorer.Run();
        _networkInterfaces = _deviceExplorer.GetNetworkInterfaces();
        _temperatureModules = _deviceExplorer.GetTemperatureModules();
        foreach (var (moduleId, networkInterface) in _networkInterfaces)
        {
            foreach (var (valueId, statistic) in networkInterface.Statistics)
            {
                _deviceReader.RegisterDevice(new DeviceInfo(moduleId, valueId, statistic.FileName, DeviceType.Network));
            }
        }

        foreach (var (moduleId, temperatureModule) in _temperatureModules)
        {
            foreach (var (deviceId, device) in temperatureModule.Devices)
            {
                _deviceReader.RegisterDevice(
                    new DeviceInfo(moduleId, deviceId, device.FileName, DeviceType.Temperature));
            }
        }

        _deviceReader.Run();
        await MainLoop();
    }

    public Dictionary<Guid, TemperatureModule> GetTemperatureModules()
    {
        return _temperatureModules;
    }

    private async Task MainLoop()
    {
        _logger.LogInformation("Main loop of MonitoringService started");
        while (true)
        {
            var value = await _valueChannel.ReadAsync();
            switch (value.DeviceType)
            {
                case DeviceType.Network:
                    _networkInterfaces[value.ModuleId].Statistics[value.ValueId].Update(value.Value);
                    //if (NetworkInterfaces[value.ModuleId].Statistics[value.ValueId].Value.GetRecord().Current != "0")
                    //{
                    //    Console.WriteLine(
                    //        $"{NetworkInterfaces[value.ModuleId].Name}\t{NetworkInterfaces[value.ModuleId].Statistics[value.ValueId].Name}\t{NetworkInterfaces[value.ModuleId].Statistics[value.ValueId].Value.GetRecord().Current}");
                    //}

                    break;
                case DeviceType.Temperature:
                    _temperatureModules[value.ModuleId].Devices[value.ValueId].Update(value.Value);
                   //Console.WriteLine(
                   //    $"{TemperatureModules[value.ModuleId].Name}\t{TemperatureModules[value.ModuleId].Devices[value.ValueId].Name}\t{TemperatureModules[value.ModuleId].Devices[value.ValueId].Value.GetRecord().Current}");

                    break;
                default:
                    break;
            }
        }
    }
}