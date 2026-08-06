using Microsoft.AspNetCore.Mvc;
using Warehouse.Services;
using Warehouse.Shared;

namespace Warehouse.Api.Controllers;

[ApiController]
[Route("api/warehouse")]
public class WarehouseController(IWarehouseService warehouseService) : ControllerBase
{
    [HttpGet(Name = "GetWarehouseRows")]
    public async Task<ActionResult<WarehousePageDto>> GetRows([FromQuery] WarehouseQuery query, CancellationToken ct)
    {
        var result = await warehouseService.GetRowsAsync(query, ct);
        return Ok(result);
    }
}
