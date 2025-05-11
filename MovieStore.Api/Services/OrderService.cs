using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MovieStore.Api.Data;
using MovieStore.Api.DTOs.Order;
using MovieStore.Api.Entities;

namespace MovieStore.Api.Services
{
    public class OrderService : IOrderService
    {
        private readonly MovieStoreDbContext _context;
        private readonly IMapper _mapper;

        public OrderService(MovieStoreDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<OrderDto>> GetAllAsync()
        {
            var orders = await _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.Movie)
                .ToListAsync();

            return _mapper.Map<List<OrderDto>>(orders);
        }

        public async Task<List<OrderDto>> GetByCustomerIdAsync(int customerId)
        {
            var orders = await _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.Movie)
                .Where(o => o.CustomerId == customerId)
                .ToListAsync();

            return _mapper.Map<List<OrderDto>>(orders);
        }

        public async Task<OrderDto> CreateAsync(int customerId, CreateOrderDto createOrderDto)
        {
            // Check if movie exists and is active
            var movie = await _context.Movies
                .FirstOrDefaultAsync(m => m.Id == createOrderDto.MovieId && m.IsActive);

            if (movie == null)
                throw new KeyNotFoundException($"Movie with ID {createOrderDto.MovieId} not found.");

            // Check if customer exists
            var customer = await _context.Customers.FindAsync(customerId);
            if (customer == null)
                throw new KeyNotFoundException($"Customer with ID {customerId} not found.");

            var order = new Order
            {
                CustomerId = customerId,
                MovieId = createOrderDto.MovieId,
                Price = movie.Price,
                PurchaseDate = DateTime.UtcNow
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            return await GetOrderDtoAsync(order.Id);
        }

        private async Task<OrderDto> GetOrderDtoAsync(int orderId)
        {
            var order = await _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.Movie)
                .FirstOrDefaultAsync(o => o.Id == orderId);

            return _mapper.Map<OrderDto>(order);
        }
    }
} 