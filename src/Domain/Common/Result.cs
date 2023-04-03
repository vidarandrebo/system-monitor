namespace Domain;

public record Result<T>(T Value, string[]? Errors);