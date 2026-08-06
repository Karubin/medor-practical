namespace Warehouse.Models;

public static class DbSeeder
{
    public static async Task SeedAsync(WarehouseDbContext db)
    {
        var categories = new[]
        {
            new Category { Name = "Elektronika" },
            new Category { Name = "Nářadí" },
            new Category { Name = "Kancelářské potřeby" },
            new Category { Name = "Potraviny" },
            new Category { Name = "Zahrada" },
            new Category { Name = "Sportovní vybavení" },
        };
        db.Categories.AddRange(categories);

        var products = new[]
        {
            new Product { Name = "Notebook" },
            new Product { Name = "Monitor" },
            new Product { Name = "Vrtačka" },
            new Product { Name = "Kladivo" },
            new Product { Name = "Sešívačka" },
            new Product { Name = "Papír A4" },
            new Product { Name = "Káva" },
            new Product { Name = "Čaj" },
            new Product { Name = "Sekačka" },
            new Product { Name = "Hadice" },
            new Product { Name = "Míč" },
            new Product { Name = "Stan" },
            new Product { Name = "Powerbanka" },
            new Product { Name = "Klávesnice" },
        };
        db.Products.AddRange(products);

        var clients = new[]
        {
            new Client { Name = "Alfa s.r.o." },
            new Client { Name = "Beta a.s." },
            new Client { Name = "Gama Trading" },
            new Client { Name = "Delta Retail" },
            new Client { Name = "Epsilon Group" },
        };
        db.Clients.AddRange(clients);

        await db.SaveChangesAsync();

        Category Cat(string name) => categories.First(c => c.Name == name);
        Product Prod(string name) => products.First(p => p.Name == name);

        var productCategoryMap = new (string Product, string[] Categories)[]
        {
            ("Notebook", new[] { "Elektronika" }),
            ("Monitor", new[] { "Elektronika" }),
            ("Vrtačka", new[] { "Nářadí" }),
            ("Kladivo", new[] { "Nářadí" }),
            ("Sešívačka", new[] { "Kancelářské potřeby" }),
            ("Papír A4", new[] { "Kancelářské potřeby" }),
            ("Káva", new[] { "Potraviny" }),
            ("Čaj", new[] { "Potraviny" }),
            ("Sekačka", new[] { "Zahrada", "Nářadí" }),
            ("Hadice", new[] { "Zahrada" }),
            ("Míč", new[] { "Sportovní vybavení" }),
            ("Stan", new[] { "Sportovní vybavení", "Zahrada" }),
            ("Powerbanka", new[] { "Elektronika", "Kancelářské potřeby" }),
            ("Klávesnice", new[] { "Elektronika", "Kancelářské potřeby" }),
        };

        foreach (var (productName, categoryNames) in productCategoryMap)
        {
            var product = Prod(productName);
            foreach (var categoryName in categoryNames)
            {
                db.ProductCategories.Add(new ProductCategory
                {
                    ProductId = product.Id,
                    CategoryId = Cat(categoryName).Id,
                });
            }
        }

        var clientCategoryMap = new (string Client, string[] Categories)[]
        {
            ("Alfa s.r.o.", new[] { "Elektronika", "Kancelářské potřeby" }),
            ("Beta a.s.", new[] { "Nářadí", "Zahrada" }),
            ("Gama Trading", new[] { "Potraviny", "Sportovní vybavení" }),
            ("Delta Retail", new[] { "Elektronika", "Sportovní vybavení", "Zahrada" }),
            ("Epsilon Group", new[] { "Kancelářské potřeby", "Nářadí" }),
        };

        foreach (var (clientName, categoryNames) in clientCategoryMap)
        {
            var client = clients.First(c => c.Name == clientName);
            foreach (var categoryName in categoryNames)
            {
                db.ClientCategories.Add(new ClientCategory
                {
                    ClientId = client.Id,
                    CategoryId = Cat(categoryName).Id,
                });
            }
        }

        await db.SaveChangesAsync();

        var random = new Random(42);

        foreach (var (clientName, clientCategoryNames) in clientCategoryMap)
        {
            var client = clients.First(c => c.Name == clientName);
            var clientCategorySet = clientCategoryNames.ToHashSet();

            foreach (var (productName, productCategoryNames) in productCategoryMap)
            {
                if (!productCategoryNames.Any(clientCategorySet.Contains))
                    continue;

                db.ClientProductStocks.Add(new ClientProductStock
                {
                    ClientId = client.Id,
                    ProductId = Prod(productName).Id,
                    Quantity = random.Next(0, 200),
                });
            }
        }

        await db.SaveChangesAsync();
    }
}
