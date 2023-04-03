using Domain.Network;

namespace Application.Interfaces;

public interface IDeviceExplorer
{
    public void Run();
    public Dictionary<Guid, NetworkInterface> GetNetworkInterfaces();
}