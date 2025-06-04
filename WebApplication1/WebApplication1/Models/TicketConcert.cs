using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Models;

[Table("Ticket_Concert")]
[PrimaryKey(nameof(TicketConcertId))]
public class TicketConcert
{
    [Key] 
    public int TicketConcertId { get; set; }

    [ForeignKey(nameof(Ticket))]  
    public int TicketId  { get; set; }
    public Ticket Ticket { get; set; } = null!;

    [ForeignKey(nameof(Concert))] 
    public int ConcertId { get; set; }
    public Concert Concert { get; set; } = null!;

    [Column(TypeName = "decimal(10,2)")]
    public decimal Price { get; set; }

    public ICollection<PurchasedTicket> PurchasedTickets { get; set; } = null!;
}