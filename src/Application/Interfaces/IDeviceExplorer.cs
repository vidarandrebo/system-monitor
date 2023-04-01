using System.Collections.Generic;
using Domain;
using Domain.Network;

namespace Application.Interfaces;

public interface IDeviceExplorer
{
    public void Run();
    public List<NetworkInterface> GetNetworkInterfaces();
}