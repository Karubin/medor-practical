using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Warehouse.Models;
using Warehouse.Shared;

namespace Warehouse.Api.Controllers;

[ApiController]
[Route("api/categories")]
public class CategoriesController(WarehouseDbContext db) : ControllerBase
{
    [HttpGet(Name = "GetCategories")]
    public async Task<ActionResult<List<CategoryDto>>> GetAll(CancellationToken ct)
    {
        var categories = await db.Categories
            .OrderBy(c => c.Name)
            .Select(c => new CategoryDto(c.Id, c.Name))
            .ToListAsync(ct);

        return Ok(categories);
    }
}
