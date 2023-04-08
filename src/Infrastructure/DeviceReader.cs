using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Channels;
using System.Threading.Tasks;
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
            var stopWatch = new Stopwatch();
            while (_running)
            {
                stopWatch.Start();
                var tasks = new List<Task>();
                foreach (var device in _devices)
                {
                    var task = Task.Run(() =>
                    {
                        var readResult = FileReader.ReadFileToString(device.FileName);
                        if (readResult.Errors != null)
                        {
                            _logger.LogError("Failed to read from file {filename}", device.FileName);
                            return;
                        }

                        _valueChannel.Writer.TryWrite(
                            new DeviceValue(
                                device.ModuleId,
                                device.ValueId,
                                readResult.Value,
                                device.DeviceType
                            )
                        );
                    });
                    tasks.Add(task);
                }

                await Task.WhenAll(tasks);

                stopWatch.Stop();
                _logger.LogInformation("Read {deviceCount} files in {timems} ms", _devices.Count,
                    stopWatch.ElapsedMilliseconds);
                stopWatch.Reset();

                await Task.Delay(2000);
            }
        });
    }

    public ChannelReader<DeviceValue> Subscribe()
    {
        return _valueChannel.Reader;
    }
}