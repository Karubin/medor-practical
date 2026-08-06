using Warehouse.Shared;

namespace Warehouse.Services;

public interface IWarehouseService
{
    Task<WarehousePageDto> GetRowsAsync(WarehouseQuery query, CancellationToken ct = default);

    Task<List<string>> GetSharedCategoryNamesAsync(int clientId, int productId, CancellationToken ct = default);
}
