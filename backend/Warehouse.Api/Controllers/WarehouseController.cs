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

    [HttpPut("{clientId:int}/{productId:int}", Name = "UpdateWarehouseQuantity")]
    public async Task<ActionResult<WarehouseRowDto>> UpdateQuantity(
        int clientId, int productId, [FromBody] UpdateQuantityDto body, CancellationToken ct)
    {
        if (body.Quantity < 0)
            return BadRequest(new { error = "Quantity must be zero or greater." });

        var updated = await warehouseService.UpdateQuantityAsync(clientId, productId, body.Quantity, ct);

        return updated is null ? NotFound() : Ok(updated);
    }
}
