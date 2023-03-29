using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Application.Interfaces;
using Domain;
using Domain.Network;
using Microsoft.Extensions.Logging;

namespace Infrastructure;

public class DeviceExplorer : IDeviceExplorer
{
    private readonly ILogger<DeviceExplorer> _logger;

    public DeviceExplorer(ILogger<DeviceExplorer> logger)
    {
        _logger = logger;
        _logger.LogInformation("DeviceExplorer Starting");
    }

    public List<DeviceInfo> Run()
    {
        FindNetworkDevices();
        return null;
    }

    private void FindNetworkDevices()
    {
        var devices = Directory.GetFileSystemEntries("/sys/class/net");
        Console.WriteLine(String.Join('\n', devices));
        foreach (var device in devices)
        {
            var name = device.Split(Path.PathSeparator).Last();
            var addressFileName = Path.Join(device, "address");
            var address = ReadFileToString(addressFileName);
            if (address.Errors != null)
            {
                _logger.LogError("Failed to read from file {addressFileName}", addressFileName);
                continue;
            }
            var networkInterface = new NetworkInterface(name, address.Value);
            var statisticsFiles = Directory.GetFileSystemEntries(Path.Join(device, "statistics"));
        }
    }

    public static Result<string> ReadFileToString(string fileName)
    {
        try
        {
            using var sr = new StreamReader(fileName);
            return new Result<string>(sr.ReadToEnd().Trim(), null);
        }
        catch (Exception ex) when (ex is IOException or OutOfMemoryException)
        {
            return new Result<string>("", new[] {ex.ToString()});
        }

        
    }
}