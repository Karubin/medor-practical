namespace Warehouse.Shared;

public class WarehouseQuery
{
    public List<int>? ClientIds { get; set; }
    public List<int>? ProductIds { get; set; }
    public List<int>? CategoryIds { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}
