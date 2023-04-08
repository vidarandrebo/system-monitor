using System;

namespace Domain;

public record DeviceValue(Guid ModuleId, Guid ValueId, string Value, DeviceType DeviceType);