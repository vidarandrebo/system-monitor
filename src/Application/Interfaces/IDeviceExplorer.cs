using Domain.Network;
using Domain.Temperature;

namespace Application.Interfaces;

public interface IDeviceExplorer
{
    public void Run();
    public Dictionary<Guid, NetworkInterface> GetNetworkInterfaces();
    public Dictionary<Guid, TemperatureModule> GetTemperatureModules();
}