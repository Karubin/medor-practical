namespace Warehouse.Models;

public class ClientProductStock
{
    public int ClientId { get; set; }
    public Client Client { get; set; } = null!;

    public int ProductId { get; set; }
    public Product Product { get; set; } = null!;

    public int Quantity { get; set; }
}
