using WebApplication1.Models;
using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Data;

public class DatabaseContext : DbContext
{
    public DbSet<Customer> Customers {get; set;}
    public DbSet<Concert> Concerts {get; set;}
    public DbSet<Ticket> Tickets {get; set;}
    public DbSet<TicketConcert> TicketConcerts {get; set;}
    public DbSet<PurchasedTicket> PurchasedTickets {get; set;}

    protected DatabaseContext() { }
    public DatabaseContext(DbContextOptions options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Customer>().HasData(
            new Customer { CustomerId = 1, FirstName = "Kazimierz", LastName = "Bankrut" },
            new Customer { CustomerId = 2, FirstName = "Adam", LastName = "Kowalski" }
        );

        modelBuilder.Entity<Concert>().HasData(
            new Concert { ConcertId = 1, Name = "ABC", Date = DateTime.Parse("2025-06-10 20:00"), AvailableTickets = 1 },
            new Concert { ConcertId = 2, Name = "DEF", Date = DateTime.Parse("2025-06-12 19:00"), AvailableTickets =1 }
        );

        modelBuilder.Entity<Ticket>().HasData(
            new Ticket { TicketId = 1, SerialNumber = "ABC1", SeatNumber = 1 },
            new Ticket { TicketId = 2, SerialNumber = "ABC2", SeatNumber = 2 }
        );

        modelBuilder.Entity<TicketConcert>().HasData(
            new TicketConcert { TicketConcertId = 1, TicketId = 1, ConcertId = 1, Price = 1.99m },
            new TicketConcert { TicketConcertId = 2, TicketId = 2, ConcertId = 2, Price = 21.37m }
        );

        modelBuilder.Entity<PurchasedTicket>().HasData(
            new PurchasedTicket { TicketConcertId = 1, CustomerId = 1, PurchaseDate = DateTime.Parse("2025-06-05 09:00") }
        );
    }
}