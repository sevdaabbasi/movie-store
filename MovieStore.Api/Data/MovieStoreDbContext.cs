using Microsoft.EntityFrameworkCore;
using MovieStore.Api.Entities;

namespace MovieStore.Api.Data
{
    public class MovieStoreDbContext : DbContext
    {
        public MovieStoreDbContext(DbContextOptions<MovieStoreDbContext> options) : base(options)
        {
        }

        public DbSet<Movie> Movies { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Actor> Actors { get; set; }
        public DbSet<Director> Directors { get; set; }
        public DbSet<MovieActor> MovieActors { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<CustomerGenre> CustomerGenres { get; set; }
        public DbSet<Order> Orders { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // MovieActor composite key
            modelBuilder.Entity<MovieActor>()
                .HasKey(ma => new { ma.MovieId, ma.ActorId });

            // CustomerGenre composite key
            modelBuilder.Entity<CustomerGenre>()
                .HasKey(cg => new { cg.CustomerId, cg.GenreId });

            // Movie - Genre relationship
            modelBuilder.Entity<Movie>()
                .HasOne(m => m.Genre)
                .WithMany(g => g.Movies)
                .HasForeignKey(m => m.GenreId)
                .OnDelete(DeleteBehavior.Restrict);

            // Movie - Director relationship
            modelBuilder.Entity<Movie>()
                .HasOne(m => m.Director)
                .WithMany(d => d.Movies)
                .HasForeignKey(m => m.DirectorId)
                .OnDelete(DeleteBehavior.Restrict);

            // Movie - MovieActor relationship
            modelBuilder.Entity<MovieActor>()
                .HasOne(ma => ma.Movie)
                .WithMany(m => m.MovieActors)
                .HasForeignKey(ma => ma.MovieId)
                .OnDelete(DeleteBehavior.Restrict);

            // Actor - MovieActor relationship
            modelBuilder.Entity<MovieActor>()
                .HasOne(ma => ma.Actor)
                .WithMany(a => a.MovieActors)
                .HasForeignKey(ma => ma.ActorId)
                .OnDelete(DeleteBehavior.Restrict);

            // Customer - Order relationship
            modelBuilder.Entity<Order>()
                .HasOne(o => o.Customer)
                .WithMany(c => c.Orders)
                .HasForeignKey(o => o.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

            // Movie - Order relationship
            modelBuilder.Entity<Order>()
                .HasOne(o => o.Movie)
                .WithMany()
                .HasForeignKey(o => o.MovieId)
                .OnDelete(DeleteBehavior.Restrict);

            // Customer - CustomerGenre relationship
            modelBuilder.Entity<CustomerGenre>()
                .HasOne(cg => cg.Customer)
                .WithMany(c => c.CustomerGenres)
                .HasForeignKey(cg => cg.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

            // Genre - CustomerGenre relationship
            modelBuilder.Entity<CustomerGenre>()
                .HasOne(cg => cg.Genre)
                .WithMany()
                .HasForeignKey(cg => cg.GenreId)
                .OnDelete(DeleteBehavior.Restrict);

            // Seed default genres
            modelBuilder.Entity<Genre>().HasData(
                new Genre { Id = 1, Name = "Action" },
                new Genre { Id = 2, Name = "Comedy" },
                new Genre { Id = 3, Name = "Drama" },
                new Genre { Id = 4, Name = "Horror" },
                new Genre { Id = 5, Name = "Sci-Fi" }
            );
        }
    }
} 