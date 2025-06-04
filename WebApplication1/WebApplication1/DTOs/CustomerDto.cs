namespace WebApplication1.DTOs;

public class CustomerDto
{
    public int    Id { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string? PhoneNumber { get; set; }
}