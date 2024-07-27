namespace TestApi.Data;

public record BookEntity(
    Guid Id,
    string Title,
    string Author);