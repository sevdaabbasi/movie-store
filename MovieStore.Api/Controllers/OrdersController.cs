using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieStore.Api.DTOs.Order;
using MovieStore.Api.Services;

namespace MovieStore.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<List<OrderDto>>> GetAll()
        {
            var orders = await _orderService.GetAllAsync();
            return Ok(orders);
        }

        [HttpGet("customer/{customerId}")]
        public async Task<ActionResult<List<OrderDto>>> GetByCustomerId(int customerId)
        {
            // Check if the requesting user is the same as the customerId or an admin
            var userId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value);
            if (userId != customerId && !User.IsInRole("Admin"))
                return Forbid();

            var orders = await _orderService.GetByCustomerIdAsync(customerId);
            return Ok(orders);
        }

        [HttpPost]
        public async Task<ActionResult<OrderDto>> Create(CreateOrderDto createOrderDto)
        {
            var userId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value);
            var order = await _orderService.CreateAsync(userId, createOrderDto);
            return CreatedAtAction(nameof(GetByCustomerId), new { customerId = userId }, order);
        }
    }
} 