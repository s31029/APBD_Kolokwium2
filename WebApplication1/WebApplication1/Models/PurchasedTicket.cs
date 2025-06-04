using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Models;

[Table("Purchased_Ticket")]
[PrimaryKey(nameof(TicketConcertId), nameof(CustomerId))]
public class PurchasedTicket
{
    [ForeignKey(nameof(TicketConcert))] 
    public int TicketConcertId { get; set; }
    public TicketConcert TicketConcert { get; set; } = null!;

    [ForeignKey(nameof(Customer))]     
    public int CustomerId { get; set; }
    public Customer Customer { get; set; } = null!;

    public DateTime PurchaseDate { get; set; }
}