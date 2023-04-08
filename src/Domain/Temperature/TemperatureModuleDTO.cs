using System;
using System.Collections.Generic;

namespace Domain.Temperature;

public record TemperatureModuleDTO(Guid ModuleId, string Name, List<DeviceDTO> Devices);