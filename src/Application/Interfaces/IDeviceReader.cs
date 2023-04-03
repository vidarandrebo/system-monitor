using System.Threading.Channels;
using Domain;

namespace Application.Interfaces;

public interface IDeviceReader
{
    void RegisterDevice(DeviceInfo deviceInfo);
    void Run();
    ChannelReader<DeviceValue> Subscribe();
}