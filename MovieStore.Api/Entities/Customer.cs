using System.Collections.Generic;

namespace MovieStore.Api.Entities
{
    public class Customer
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public ICollection<Order> Orders { get; set; }
        public ICollection<CustomerGenre> CustomerGenres { get; set; }
    }
} 