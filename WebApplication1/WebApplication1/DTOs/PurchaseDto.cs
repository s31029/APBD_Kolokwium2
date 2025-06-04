namespace WebApplication1.DTOs;

public class PurchaseDto
{
    public DateTime Date  { get; set; }
    public decimal  Price { get; set; }

    public TicketInfoDto  Ticket  { get; set; } = null!;
    public ConcertInfoDto Concert { get; set; } = null!;
}