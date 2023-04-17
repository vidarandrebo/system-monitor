using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Temperature;

namespace Application.Interfaces;

public interface IMonitoringService
{
    Task Run();
    Dictionary<Guid, TemperatureModule> GetTemperatureModules();
    List<ModuleDTO> GetModuleDTOs();
}