using System.Threading.Channels;
using Application.Interfaces;
using Domain;
using Domain.Network;
using Domain.Temperature;

namespace Application;

public interface IMonitoringService
{
    public Task Run();
}

public class MonitoringService : IMonitoringService
{
    private readonly IDeviceExplorer _deviceExplorer;
    private readonly IDeviceReader _deviceReader;
    private ChannelReader<DeviceValue> _valueChannel;
    public Dictionary<Guid, NetworkInterface> NetworkInterfaces;
    public Dictionary<Guid, TemperatureModule> TemperatureModules;

    public MonitoringService(IDeviceExplorer deviceExplorer, IDeviceReader deviceReader)
    {
        _deviceExplorer = deviceExplorer;
        _deviceReader = deviceReader;
        NetworkInterfaces = new Dictionary<Guid, NetworkInterface>();
        TemperatureModules = new Dictionary<Guid, TemperatureModule>();
        _valueChannel = _deviceReader.Subscribe();
    }

    public async Task Run()
    {
        _deviceExplorer.Run();
        NetworkInterfaces = _deviceExplorer.GetNetworkInterfaces();
        TemperatureModules = _deviceExplorer.GetTemperatureModules();
        foreach (var (moduleId, networkInterface) in NetworkInterfaces)
        {
            foreach (var (valueId, statistic) in networkInterface.Statistics)
            {
                _deviceReader.RegisterDevice(new DeviceInfo(moduleId, valueId, statistic.FileName, DeviceType.Network));
            }
        }

        foreach (var (moduleId, temperatureModule) in TemperatureModules)
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

    private async Task MainLoop()
    {
        while (true)
        {
            var value = await _valueChannel.ReadAsync();
            switch (value.DeviceType)
            {
                case DeviceType.Network:
                    NetworkInterfaces[value.ModuleId].Statistics[value.ValueId].Update(value.Value);
                   //if (NetworkInterfaces[value.ModuleId].Statistics[value.ValueId].Value.GetRecord().Current != "0")
                   //{
                   //    Console.WriteLine(
                   //        $"{NetworkInterfaces[value.ModuleId].Name}\t{NetworkInterfaces[value.ModuleId].Statistics[value.ValueId].Name}\t{NetworkInterfaces[value.ModuleId].Statistics[value.ValueId].Value.GetRecord().Current}");
                   //}

                    break;
                case DeviceType.Temperature:
                    TemperatureModules[value.ModuleId].Devices[value.ValueId].Update(value.Value);
                   //Console.WriteLine(
                   //    $"{TemperatureModules[value.ModuleId].Name}\t{TemperatureModules[value.ModuleId].Devices[value.ValueId].Name}\t{TemperatureModules[value.ModuleId].Devices[value.ValueId].Value.GetRecord().Current}");

                    break;
                default:
                    break;
            }
        }
    }
}