using System;
using System.Collections.Generic;
using System.Linq;
using Application.Interfaces;
using Domain;
using Domain.Temperature;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace WebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class DataController : ControllerBase
{

    private readonly ILogger<DataController> _logger;
    private readonly IMonitoringService _monitoringService;

    public DataController(ILogger<DataController> logger, IMonitoringService monitoringService)
    {
        _logger = logger;
        _monitoringService = monitoringService;
    }

    [HttpGet]
    public ActionResult<List<TemperatureModuleDTO>> Get()
    {
        _logger.LogInformation("In Datacontroller");
        var moduleDTOs = new List<TemperatureModuleDTO>();
        var modules = _monitoringService.GetTemperatureModules();
        foreach (var (moduleId, module) in modules.OrderBy(m => m.Value.Name))
        {
            var deviceDTOs = new List<DeviceDTO>();
            foreach (var (deviceId, device) in module.Devices.OrderBy(d => d.Value.Name))
            {
                deviceDTOs.Add(new DeviceDTO(deviceId, device.Name, device.Value.GetRecord()));
            }

            moduleDTOs.Add(new TemperatureModuleDTO(moduleId, module.Name, deviceDTOs));
        }

        return moduleDTOs;
    }
}