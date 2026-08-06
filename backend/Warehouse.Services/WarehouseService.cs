using Microsoft.EntityFrameworkCore;
using Warehouse.Models;
using Warehouse.Shared;

namespace Warehouse.Services;

public class WarehouseService(WarehouseDbContext db) : IWarehouseService
{
    public async Task<WarehousePageDto> GetRowsAsync(WarehouseQuery query, CancellationToken ct = default)
    {
        var stocksQuery = db.ClientProductStocks.AsQueryable();

        if (query.ClientIds is { Count: > 0 })
            stocksQuery = stocksQuery.Where(s => query.ClientIds.Contains(s.ClientId));

        if (query.ProductIds is { Count: > 0 })
            stocksQuery = stocksQuery.Where(s => query.ProductIds.Contains(s.ProductId));

        if (query.CategoryIds is { Count: > 0 })
        {
            var matchingPairs = db.ProductCategories
                .Join(db.ClientCategories, pc => pc.CategoryId, cc => cc.CategoryId,
                    (pc, cc) => new { cc.ClientId, pc.ProductId, pc.CategoryId })
                .Where(x => query.CategoryIds.Contains(x.CategoryId));

            stocksQuery = stocksQuery.Where(s =>
                matchingPairs.Any(m => m.ClientId == s.ClientId && m.ProductId == s.ProductId));
        }

        var joined =
            from s in stocksQuery
            join c in db.Clients on s.ClientId equals c.Id
            join p in db.Products on s.ProductId equals p.Id
            orderby c.Name, p.Name
            select new { s.ClientId, ClientName = c.Name, s.ProductId, ProductName = p.Name, s.Quantity };

        var totalCount = await joined.CountAsync(ct);

        var page = query.Page < 1 ? 1 : query.Page;
        var pageSize = query.PageSize is < 1 or > 100 ? 10 : query.PageSize;

        var pageRows = await joined
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);

        var categoriesByPair = await LoadCategoriesForPairsAsync(pageRows.Select(r => (r.ClientId, r.ProductId)), ct);

        var items = pageRows
            .Select(row => new WarehouseRowDto(
                row.ClientId,
                row.ClientName,
                row.ProductId,
                row.ProductName,
                categoriesByPair.GetValueOrDefault((row.ClientId, row.ProductId), []),
                row.Quantity))
            .ToList();

        return new WarehousePageDto(items, totalCount, page, pageSize);
    }

    private async Task<Dictionary<(int ClientId, int ProductId), List<string>>> LoadCategoriesForPairsAsync(
        IEnumerable<(int ClientId, int ProductId)> pairs, CancellationToken ct)
    {
        var distinctPairs = pairs.Distinct().ToList();
        var clientIds = distinctPairs.Select(p => p.ClientId).Distinct().ToList();
        var productIds = distinctPairs.Select(p => p.ProductId).Distinct().ToList();

        var rows = await (
            from pc in db.ProductCategories
            join cc in db.ClientCategories on pc.CategoryId equals cc.CategoryId
            join cat in db.Categories on pc.CategoryId equals cat.Id
            where productIds.Contains(pc.ProductId) && clientIds.Contains(cc.ClientId)
            select new { cc.ClientId, pc.ProductId, CategoryName = cat.Name })
            .ToListAsync(ct);

        return rows
            .GroupBy(r => (r.ClientId, r.ProductId))
            .ToDictionary(g => g.Key, g => g.Select(r => r.CategoryName).OrderBy(n => n).ToList());
    }

    public async Task<List<string>> GetSharedCategoryNamesAsync(int clientId, int productId, CancellationToken ct = default)
    {
        return await (
            from pc in db.ProductCategories
            join cc in db.ClientCategories on pc.CategoryId equals cc.CategoryId
            join cat in db.Categories on pc.CategoryId equals cat.Id
            where pc.ProductId == productId && cc.ClientId == clientId
            orderby cat.Name
            select cat.Name)
            .ToListAsync(ct);
    }

    public async Task<WarehouseRowDto?> UpdateQuantityAsync(int clientId, int productId, int quantity, CancellationToken ct = default)
    {
        var stock = await db.ClientProductStocks
            .FirstOrDefaultAsync(s => s.ClientId == clientId && s.ProductId == productId, ct);

        if (stock is null)
            return null;

        stock.Quantity = quantity;
        await db.SaveChangesAsync(ct);

        var client = await db.Clients.FirstAsync(c => c.Id == clientId, ct);
        var product = await db.Products.FirstAsync(p => p.Id == productId, ct);
        var categories = await GetSharedCategoryNamesAsync(clientId, productId, ct);

        return new WarehouseRowDto(clientId, client.Name, productId, product.Name, categories, stock.Quantity);
    }
}
