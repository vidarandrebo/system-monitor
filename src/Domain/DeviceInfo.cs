using System;

namespace Domain;

public record DeviceInfo(Guid ModuleId, Guid ValueId, string FileName, DeviceType DeviceType);