using System;

namespace MovieStore.Api.DTOs.Order
{
    public class OrderDto
    {
        public int Id { get; set; }
        public string CustomerName { get; set; }
        public string MovieName { get; set; }
        public decimal Price { get; set; }
        public DateTime PurchaseDate { get; set; }
    }

    public class CreateOrderDto
    {
        public int MovieId { get; set; }
    }
} 