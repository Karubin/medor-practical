namespace Warehouse.Shared;

public record WarehousePageDto(
    List<WarehouseRowDto> Items,
    int TotalCount,
    int Page,
    int PageSize);
