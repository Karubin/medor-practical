using Microsoft.EntityFrameworkCore;

namespace Warehouse.Models;

public class WarehouseDbContext(DbContextOptions<WarehouseDbContext> options) : DbContext(options)
{
    public DbSet<Client> Clients => Set<Client>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Product> Products => Set<Product>();
    public DbSet<ProductCategory> ProductCategories => Set<ProductCategory>();
    public DbSet<ClientCategory> ClientCategories => Set<ClientCategory>();
    public DbSet<ClientProductStock> ClientProductStocks => Set<ClientProductStock>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ProductCategory>().HasKey(x => new { x.ProductId, x.CategoryId });
        modelBuilder.Entity<ClientCategory>().HasKey(x => new { x.ClientId, x.CategoryId });
        modelBuilder.Entity<ClientProductStock>().HasKey(x => new { x.ClientId, x.ProductId });
    }
}
