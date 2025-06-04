namespace WebApplication1.DTOs;

public class NewPurchaseDto
{
    public int SeatNumber  { get; set; }
    public string ConcertName { get; set; } = null!;
    public decimal Price { get; set; }
}