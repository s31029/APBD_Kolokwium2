namespace WebApplication1.DTOs;

public class NewCustomerRequestDto
{
    public CustomerDto Customer { get; set; } = null!;
    public List<NewPurchaseDto>  Purchases { get; set; } = new();
}