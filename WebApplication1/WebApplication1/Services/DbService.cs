using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.DTOs;
using WebApplication1.Exceptions;
using WebApplication1.Models;
using WebApplication1.Services;

public class DbService : IDbService
{
    private readonly DatabaseContext _ctx;
    public DbService(DatabaseContext ctx) => _ctx = ctx;
    public async Task<CustomerPurchasesDto> GetPurchases(int customerId)
    {
        var result = await _ctx.Customers
            .Where(c => c.CustomerId == customerId)
            .Select(c => new CustomerPurchasesDto
            {
                FirstName   = c.FirstName,
                LastName    = c.LastName,
                PhoneNumber = c.PhoneNumber,
                Purchases   = c.PurchasedTickets.Select(pt => new PurchaseDto
                {
                    Date  = pt.PurchaseDate,
                    Price = pt.TicketConcert.Price,
                    Ticket = new TicketInfoDto
                    {
                        Serial     = pt.TicketConcert.Ticket.SerialNumber,
                        SeatNumber = pt.TicketConcert.Ticket.SeatNumber
                    },
                    Concert = new ConcertInfoDto
                    {
                        Name = pt.TicketConcert.Concert.Name,
                        Date = pt.TicketConcert.Concert.Date
                    }
                }).ToList()
            })
            .FirstOrDefaultAsync();

        if (result is null) throw new NotFoundException("Customer not found");
        return result;
    }
    
    public async Task AddCustomerWithPurchases(NewCustomerRequestDto dto)
    {
        using var tx = await _ctx.Database.BeginTransactionAsync();

        try
        {
            var customer = await _ctx.Customers
                .FirstOrDefaultAsync(c => c.CustomerId == dto.Customer.Id);

            if (customer is null)
            {
                customer = new Customer
                {
                    CustomerId  = dto.Customer.Id,
                    FirstName   = dto.Customer.FirstName,
                    LastName    = dto.Customer.LastName,
                    PhoneNumber = dto.Customer.PhoneNumber
                };
                _ctx.Customers.Add(customer);
            }
            
            var grouped = dto.Purchases.GroupBy(p => p.ConcertName);
            foreach (var g in grouped)
                if (g.Count() > 5)
                    throw new ConflictException($"Cannot buy more than 5 tickets for '{g.Key}'");
            
            foreach (var purchase in dto.Purchases)
            {
                var concert = await _ctx.Concerts
                    .Include(c => c.TicketConcerts)
                    .FirstOrDefaultAsync(c => c.Name == purchase.ConcertName);

                if (concert is null)
                    throw new NotFoundException($"Concert '{purchase.ConcertName}' not found");

                var owned = await _ctx.PurchasedTickets
                    .CountAsync(pt => pt.CustomerId == customer.CustomerId &&
                                      pt.TicketConcert.ConcertId == concert.ConcertId);
                if (owned >= 5)
                    throw new ConflictException($"Limit reached for '{concert.Name}'");
                
                var ticket = new Ticket
                {
                    SerialNumber = Guid.NewGuid().ToString(),
                    SeatNumber   = purchase.SeatNumber
                };
                _ctx.Tickets.Add(ticket);
                
                var ticketConcert = new TicketConcert
                {
                    Ticket   = ticket,
                    Concert  = concert,
                    Price    = purchase.Price
                };
                _ctx.TicketConcerts.Add(ticketConcert);
                
                var purchased = new PurchasedTicket
                {
                    TicketConcert = ticketConcert,
                    Customer      = customer,
                    PurchaseDate  = DateTime.Now
                };
                _ctx.PurchasedTickets.Add(purchased);
            }

            await _ctx.SaveChangesAsync();
            await tx.CommitAsync();
        }
        catch
        {
            await tx.RollbackAsync();
            throw;
        }
    }
}