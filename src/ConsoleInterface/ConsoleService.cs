using Application.Interfaces;

namespace ConsoleInterface;

public interface IConsoleService
{
    Task Run();
}

public class ConsoleService : IConsoleService
{
    private readonly IMonitoringService _monitoringService;

    public ConsoleService(IMonitoringService monitoringService)
    {
        _monitoringService = monitoringService;
    }

    public async Task Run()
    {
        await _monitoringService.Run();
    }
}