using System.Threading.Channels;
using Application;
using Application.Interfaces;
using Domain;
using Microsoft.Extensions.Logging;

namespace Infrastructure;

public class DeviceReader : IDeviceReader
{
    private readonly ILogger<DeviceReader> _logger;
    private Channel<DeviceValue> _valueChannel;
    private List<DeviceInfo> _devices;
    private bool _running;

    public DeviceReader(ILogger<DeviceReader> logger)
    {
        _logger = logger;
        _valueChannel = Channel.CreateUnbounded<DeviceValue>();
        _devices = new List<DeviceInfo>();
        _running = false;
        _logger.LogInformation("Created instance of DeviceReader");
    }

    public void RegisterDevice(DeviceInfo deviceInfo)
    {
        _devices.Add(deviceInfo);
    }

    public void Run()
    {
        _running = true;
        var delay = new TimeSpan(0, 0, 0, 0, 0, 2000000 / _devices.Count);
        Task.Run(async () =>
        {
            while (_running)
            {
                foreach (var device in _devices)
                {
                    var readResult = FileReader.ReadFileToString(device.FileName);
                    if (readResult.Errors != null)
                    {
                        _logger.LogError("Failed to read from file {filename}", device.FileName);
                        continue;
                    }

                    _valueChannel.Writer.TryWrite(
                        new DeviceValue(
                            device.ModuleId,
                            device.ValueId,
                            readResult.Value,
                            device.DeviceType
                        )
                    );
                    //await Task.Delay(delay);
                }

                await Task.Delay(2000);
            }
        });
    }

    public ChannelReader<DeviceValue> Subscribe()
    {
        return _valueChannel.Reader;
    }
}