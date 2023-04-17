using System;
using System.Collections.Generic;

namespace Domain.Temperature;

public record ModuleDTO(Guid ModuleId, string Name, List<DeviceDTO> Devices);