namespace Warehouse.Models;

public class ClientCategory
{
    public int ClientId { get; set; }
    public Client Client { get; set; } = null!;

    public int CategoryId { get; set; }
    public Category Category { get; set; } = null!;
}
