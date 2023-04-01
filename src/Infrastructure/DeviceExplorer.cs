using Application;
using Application.Interfaces;
using Domain;
using Domain.Network;
using Microsoft.Extensions.Logging;

namespace Infrastructure;

public class DeviceExplorer : IDeviceExplorer
{
    private readonly ILogger<DeviceExplorer> _logger;
    private List<NetworkInterface> _networkInterfaces;

    public DeviceExplorer(ILogger<DeviceExplorer> logger)
    {
        _logger = logger;
        _networkInterfaces = new List<NetworkInterface>();
        _logger.LogInformation("DeviceExplorer Starting");
    }

    public void Run()
    {
        FindNetworkInterfaces();
    }

    public List<NetworkInterface> GetNetworkInterfaces()
    {
        return _networkInterfaces;
    }

    private void FindNetworkInterfaces()
    {
        var devices = Directory.GetFileSystemEntries("/sys/class/net");
        Console.WriteLine(String.Join('\n', devices));
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
            
        }
    }

    public void FindDeviceStatistics(NetworkInterface networkInterface, string[] filePaths)
    {
        Console.WriteLine(networkInterface.Name);
        foreach (var path in filePaths)
        {
            var fileContent = FileReader.ReadFileToString(path);
            var name = path.Split(Path.DirectorySeparatorChar).Last();
            var statistic = new Statistic(name, path);
            if (name.Contains("bytes"))
            {
                statistic.Value = new ByteValue();
                statistic.Update(fileContent.Value);
                Console.WriteLine(statistic.Value.GetRecord());
            }
            else
            {
                statistic.Value = new IntegerValue();
                statistic.Update(fileContent.Value);
            }

            networkInterface.AddStatistic(statistic);
        }

    }

}