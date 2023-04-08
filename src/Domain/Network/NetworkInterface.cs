using System;
using System.Collections.Generic;

namespace Domain.Network;

public class NetworkInterface
{
    public NetworkInterface(string name, string macAddress)
    {
        Name = name;
        MacAddress = macAddress;
        Statistics = new Dictionary<Guid, Device>();
    }

    public readonly string Name;
    public readonly string MacAddress;
    public Dictionary<Guid, Device> Statistics;

    public void AddStatistic(Device statistic)
    {
        Statistics.Add(Guid.NewGuid(), statistic);
    }

    public GuidToFilename[] GetFiles()
    {
        var files = new List<GuidToFilename>();
        foreach (var id in Statistics.Keys)
        {
            files.Add(new GuidToFilename(id, Statistics[id].FileName));
        }

        return files.ToArray();
    }
}