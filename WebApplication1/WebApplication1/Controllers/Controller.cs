using WebApplication1.DTOs;
using WebApplication1.Exceptions;
using WebApplication1.Services;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers;

[ApiController]
[Route("api/customers")]
public class CustomersController : ControllerBase
{
    private readonly IDbService _svc;
    public CustomersController(IDbService svc) => _svc = svc;
    
    [HttpGet("{id}/purchases")]
    public async Task<IActionResult> GetPurchases(int id)
    {
        try { return Ok(await _svc.GetPurchases(id)); }
        catch (NotFoundException e) { return NotFound(e.Message); }
    }
    
    [HttpPost]
    public async Task<IActionResult> AddCustomer([FromBody] NewCustomerRequestDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        try
        {
            await _svc.AddCustomerWithPurchases(dto);
            return StatusCode(StatusCodes.Status201Created);
        }
        catch (NotFoundException e)   { return NotFound(e.Message); }
        catch (ConflictException e)   { return Conflict(e.Message); }
        catch (BadRequestException e) { return BadRequest(e.Message); }
    }
}