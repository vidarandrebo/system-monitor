using System.Collections.Generic;
using Application.Interfaces;
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
        return _monitoringService.GetTemperatureModuleDTOs();
    }
}