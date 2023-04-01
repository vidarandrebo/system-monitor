namespace Application.Interfaces;

public interface IDeviceReader
{
    void RegisterDevice(Guid id, string filename);
    string ReadDevice(Guid id);
    string[] ReadDevices(IEnumerable<Guid> ids);
}