namespace Warehouse.Shared;

public record WarehouseRowDto(
    int ClientId,
    string ClientName,
    int ProductId,
    string ProductName,
    List<string> Categories,
    int Quantity);
