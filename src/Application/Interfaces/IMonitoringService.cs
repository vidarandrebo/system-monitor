using Domain.Temperature;

namespace Application.Interfaces;

public interface IMonitoringService
{
    Task Run();
    Dictionary<Guid, TemperatureModule> GetTemperatureModules();
    List<TemperatureModuleDTO> GetTemperatureModuleDTOs();
}