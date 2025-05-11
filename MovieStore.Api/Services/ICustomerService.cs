using System.Collections.Generic;
using System.Threading.Tasks;
using MovieStore.Api.DTOs.Customer;

namespace MovieStore.Api.Services
{
    public interface ICustomerService
    {
        Task<List<CustomerDto>> GetAllAsync();
        Task<CustomerDto> GetByIdAsync(int id);
        Task<CustomerDto> CreateAsync(CreateCustomerDto createCustomerDto);
        Task DeleteAsync(int id);
        Task<AuthResponseDto> LoginAsync(LoginDto loginDto);
    }
} 