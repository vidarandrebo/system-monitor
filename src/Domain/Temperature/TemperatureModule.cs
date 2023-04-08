using System;
using System.Collections.Generic;

namespace Domain.Temperature;

public class TemperatureModule
{
    public readonly string Name;
    public Dictionary<Guid, Device> Devices;

    public void AddDevice(Device device)
    {
        Devices.Add(Guid.NewGuid(), device);
    }

    public TemperatureModule(string name)
    {
        Name = name;
        Devices = new Dictionary<Guid, Device>();
    }
}