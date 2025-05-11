using System;

namespace MovieStore.Api.Entities
{
    public class Order
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public int MovieId { get; set; }
        public decimal Price { get; set; }
        public DateTime PurchaseDate { get; set; }
        public Customer Customer { get; set; }
        public Movie Movie { get; set; }
    }
} 