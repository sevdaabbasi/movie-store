using System.Collections.Generic;
using System.Threading.Tasks;
using MovieStore.Api.DTOs.Order;

namespace MovieStore.Api.Services
{
    public interface IOrderService
    {
        Task<List<OrderDto>> GetAllAsync();
        Task<List<OrderDto>> GetByCustomerIdAsync(int customerId);
        Task<OrderDto> CreateAsync(int customerId, CreateOrderDto createOrderDto);
    }
} 