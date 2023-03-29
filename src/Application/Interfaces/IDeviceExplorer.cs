using Domain;

namespace Application.Interfaces;

public interface IDeviceExplorer
{
    public List<DeviceInfo> Run();
}