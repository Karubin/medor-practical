using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Warehouse.Models;
using Warehouse.Shared;

namespace Warehouse.Api.Controllers;

[ApiController]
[Route("api/clients")]
public class ClientsController(WarehouseDbContext db) : ControllerBase
{
    [HttpGet(Name = "GetClients")]
    public async Task<ActionResult<List<ClientDto>>> GetAll(CancellationToken ct)
    {
        var clients = await db.Clients
            .OrderBy(c => c.Name)
            .Select(c => new ClientDto(c.Id, c.Name))
            .ToListAsync(ct);

        return Ok(clients);
    }
}
