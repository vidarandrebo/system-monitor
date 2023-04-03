using System.Threading.Channels;
using Application.Interfaces;
using Domain;
using Domain.Network;

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

    public MonitoringService(IDeviceExplorer deviceExplorer, IDeviceReader deviceReader)
    {
        _deviceExplorer = deviceExplorer;
        _deviceReader = deviceReader;
        NetworkInterfaces = new Dictionary<Guid, NetworkInterface>();
        _valueChannel = _deviceReader.Subscribe();
    }

    public async Task Run()
    {
        _deviceExplorer.Run();
        NetworkInterfaces = _deviceExplorer.GetNetworkInterfaces();
        foreach (var (moduleId, networkInterface) in NetworkInterfaces)
        {
            foreach (var (valueId, statistic) in networkInterface.Statistics)
            {
                _deviceReader.RegisterDevice(new DeviceInfo(moduleId, valueId, statistic.FileName, DeviceType.Network));
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
                    if (NetworkInterfaces[value.ModuleId].Statistics[value.ValueId].Value.GetRecord().Current != "0")
                    {
                        Console.WriteLine(
                            $"{NetworkInterfaces[value.ModuleId].Name},{NetworkInterfaces[value.ModuleId].Statistics[value.ValueId].Name}, {NetworkInterfaces[value.ModuleId].Statistics[value.ValueId].Value.GetRecord().Current}");
                    }

                    break;
                default:
                    break;
            }
        }
    }
}