namespace Domain;

public class MonitoringModule
{
    public Guid Id;
    public string Name;
    public readonly List<DeviceTemperature> DeviceTemperatures;

    public MonitoringModule(string name)
    {
        DeviceTemperatures = new List<DeviceTemperature>();
        Name = name;
        Id = Guid.NewGuid();
    }

    public void AddDevice(DeviceTemperature device)
    {
        DeviceTemperatures.Add(device);
    }
}