using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MovieStore.Api.Data;
using MovieStore.Api.DTOs.Customer;
using MovieStore.Api.Entities;
using System.IdentityModel.Tokens.Jwt;

namespace MovieStore.Api.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly MovieStoreDbContext _context;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        public CustomerService(MovieStoreDbContext context, IMapper mapper, IConfiguration configuration)
        {
            _context = context;
            _mapper = mapper;
            _configuration = configuration;
        }

        public async Task<List<CustomerDto>> GetAllAsync()
        {
            var customers = await _context.Customers
                .Include(c => c.Orders)
                    .ThenInclude(o => o.Movie)
                .Include(c => c.CustomerGenres)
                    .ThenInclude(cg => cg.Genre)
                .ToListAsync();

            return _mapper.Map<List<CustomerDto>>(customers);
        }

        public async Task<CustomerDto> GetByIdAsync(int id)
        {
            var customer = await _context.Customers
                .Include(c => c.Orders)
                    .ThenInclude(o => o.Movie)
                .Include(c => c.CustomerGenres)
                    .ThenInclude(cg => cg.Genre)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (customer == null)
                throw new KeyNotFoundException($"Customer with ID {id} not found.");

            return _mapper.Map<CustomerDto>(customer);
        }

        public async Task<CustomerDto> CreateAsync(CreateCustomerDto createCustomerDto)
        {
            // Check if email already exists
            if (await _context.Customers.AnyAsync(c => c.Email == createCustomerDto.Email))
                throw new InvalidOperationException("Email already exists.");

            var customer = _mapper.Map<Customer>(createCustomerDto);

            // Hash password
            CreatePasswordHash(createCustomerDto.Password, out byte[] passwordHash, out byte[] passwordSalt);
            customer.PasswordHash = passwordHash;
            customer.PasswordSalt = passwordSalt;

            // Add favorite genres
            if (createCustomerDto.FavoriteGenreIds != null)
            {
                customer.CustomerGenres = createCustomerDto.FavoriteGenreIds.Select(genreId => new CustomerGenre
                {
                    GenreId = genreId
                }).ToList();
            }

            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();

            return await GetByIdAsync(customer.Id);
        }

        public async Task DeleteAsync(int id)
        {
            var customer = await _context.Customers
                .Include(c => c.Orders)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (customer == null)
                throw new KeyNotFoundException($"Customer with ID {id} not found.");

            // Check if customer has any orders
            if (customer.Orders.Any())
                throw new InvalidOperationException("Cannot delete customer with existing orders.");

            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync();
        }

        public async Task<AuthResponseDto> LoginAsync(LoginDto loginDto)
        {
            var customer = await _context.Customers
                .Include(c => c.Orders)
                    .ThenInclude(o => o.Movie)
                .Include(c => c.CustomerGenres)
                    .ThenInclude(cg => cg.Genre)
                .FirstOrDefaultAsync(c => c.Email == loginDto.Email);

            if (customer == null)
                throw new KeyNotFoundException("Invalid email or password.");

            if (!VerifyPasswordHash(loginDto.Password, customer.PasswordHash, customer.PasswordSalt))
                throw new KeyNotFoundException("Invalid email or password.");

            var token = GenerateJwtToken(customer);
            var customerDto = _mapper.Map<CustomerDto>(customer);

            return new AuthResponseDto
            {
                Token = token,
                Customer = customerDto
            };
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }
        }

        private string GenerateJwtToken(Customer customer)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, customer.Id.ToString()),
                    new Claim(ClaimTypes.Email, customer.Email)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
} 