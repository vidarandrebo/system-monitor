using System;
using System.Threading.Tasks;
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
        Task.Run(() => _monitoringService.Run());
        while (true)
        {
            var tempModules = _monitoringService.GetModuleDTOs();
            foreach (var module in tempModules)
            {
                foreach (var  device in module.Devices)
                {
                    Console.WriteLine($"{module.Name}, {device.Name}, {device.Value.Current}");
                }
            }

            await Task.Delay(2000);
        }
    }
}