using WebApplication1.DTOs;

namespace WebApplication1.Services;

public interface IDbService
{
    Task<CustomerPurchasesDto> GetPurchases(int customerId);
    Task AddCustomerWithPurchases(NewCustomerRequestDto dto);
}