using System;

namespace Domain;

public record DeviceDTO(Guid DeviceId, string Name, ValueDTO Value);