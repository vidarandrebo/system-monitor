using Application.Interfaces;

namespace Infrastructure;

public class DeviceReader : IDeviceReader
{
    public void RegisterDevice(Guid id, string filename)
    {
        throw new NotImplementedException();
    }

    public string ReadDevice(Guid id)
    {
        throw new NotImplementedException();
    }

    public string[] ReadDevices(IEnumerable<Guid> ids)
    {
        throw new NotImplementedException();
    }
}