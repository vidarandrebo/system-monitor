using Application.Interfaces;

namespace ConsoleInterface;

public interface IConsoleService
{
    void Run();
}

public class ConsoleService : IConsoleService
{
    private readonly IDeviceExplorer _deviceExplorer;
    private readonly IDeviceReader _deviceReader;

    public ConsoleService(IDeviceExplorer deviceExplorer, IDeviceReader deviceReader)
    {
        _deviceExplorer = deviceExplorer;
        _deviceReader = deviceReader;
    }

    public void Run()
    {
        _deviceExplorer.Run();
    }
}