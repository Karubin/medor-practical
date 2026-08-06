using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Warehouse.Models;
using Warehouse.Shared;

namespace Warehouse.Api.Controllers;

[ApiController]
[Route("api/products")]
public class ProductsController(WarehouseDbContext db) : ControllerBase
{
    [HttpGet(Name = "GetProducts")]
    public async Task<ActionResult<List<ProductDto>>> GetAll(CancellationToken ct)
    {
        var products = await db.Products
            .OrderBy(p => p.Name)
            .Select(p => new ProductDto(p.Id, p.Name))
            .ToListAsync(ct);

        return Ok(products);
    }
}
