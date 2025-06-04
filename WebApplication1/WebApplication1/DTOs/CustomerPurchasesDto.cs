namespace WebApplication1.DTOs;

public class CustomerPurchasesDto
{
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string? PhoneNumber { get; set; }

    public List<PurchaseDto> Purchases { get; set; } = new();
}