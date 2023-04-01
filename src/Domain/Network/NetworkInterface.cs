namespace Domain.Network;

public class NetworkInterface
{
    public NetworkInterface(string name, string macAddress)
    {
        Name = name;
        MacAddress = macAddress;
        Statistics = new Dictionary<Guid, Statistic>();
    }

    public string Name { get; set; }
    public string MacAddress { get; set; }
    public Dictionary<Guid, Statistic> Statistics;

    public void AddStatistic(Statistic statistic)
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