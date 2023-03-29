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
}