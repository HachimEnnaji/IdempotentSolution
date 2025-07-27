namespace IdempotentApi.Domain;

public class CreateOrderRequest
{
    public string Name { get; set; } = string.Empty;
    public int Quantity { get; set; }
}
