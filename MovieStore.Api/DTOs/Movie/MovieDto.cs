using System.Collections.Generic;

namespace MovieStore.Api.DTOs.Movie
{
    public class MovieDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Year { get; set; }
        public decimal Price { get; set; }
        public string GenreName { get; set; }
        public string DirectorName { get; set; }
        public List<string> ActorNames { get; set; }
    }

    public class CreateMovieDto
    {
        public string Name { get; set; }
        public int Year { get; set; }
        public decimal Price { get; set; }
        public int GenreId { get; set; }
        public int DirectorId { get; set; }
        public List<int> ActorIds { get; set; }
    }

    public class UpdateMovieDto
    {
        public string Name { get; set; }
        public int Year { get; set; }
        public decimal Price { get; set; }
        public int GenreId { get; set; }
        public int DirectorId { get; set; }
        public List<int> ActorIds { get; set; }
    }
} 