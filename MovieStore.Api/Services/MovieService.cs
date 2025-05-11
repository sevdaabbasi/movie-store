using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MovieStore.Api.Data;
using MovieStore.Api.DTOs.Movie;
using MovieStore.Api.Entities;

namespace MovieStore.Api.Services
{
    public class MovieService : IMovieService
    {
        private readonly MovieStoreDbContext _context;
        private readonly IMapper _mapper;

        public MovieService(MovieStoreDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<MovieDto>> GetAllAsync()
        {
            var movies = await _context.Movies
                .Include(m => m.Genre)
                .Include(m => m.Director)
                .Include(m => m.MovieActors)
                    .ThenInclude(ma => ma.Actor)
                .Where(m => m.IsActive)
                .ToListAsync();

            return _mapper.Map<List<MovieDto>>(movies);
        }

        public async Task<MovieDto> GetByIdAsync(int id)
        {
            var movie = await _context.Movies
                .Include(m => m.Genre)
                .Include(m => m.Director)
                .Include(m => m.MovieActors)
                    .ThenInclude(ma => ma.Actor)
                .FirstOrDefaultAsync(m => m.Id == id && m.IsActive);

            if (movie == null)
                throw new KeyNotFoundException($"Movie with ID {id} not found.");

            return _mapper.Map<MovieDto>(movie);
        }

        public async Task<MovieDto> CreateAsync(CreateMovieDto createMovieDto)
        {
            var movie = _mapper.Map<Entities.Movie>(createMovieDto);
            movie.IsActive = true;

            // Add movie actors
            if (createMovieDto.ActorIds != null)
            {
                movie.MovieActors = createMovieDto.ActorIds.Select(actorId => new MovieActor
                {
                    ActorId = actorId
                }).ToList();
            }

            _context.Movies.Add(movie);
            await _context.SaveChangesAsync();

            return await GetByIdAsync(movie.Id);
        }

        public async Task<MovieDto> UpdateAsync(int id, UpdateMovieDto updateMovieDto)
        {
            var movie = await _context.Movies
                .Include(m => m.MovieActors)
                .FirstOrDefaultAsync(m => m.Id == id && m.IsActive);

            if (movie == null)
                throw new KeyNotFoundException($"Movie with ID {id} not found.");

            _mapper.Map(updateMovieDto, movie);

            // Update movie actors
            if (updateMovieDto.ActorIds != null)
            {
                movie.MovieActors.Clear();
                movie.MovieActors = updateMovieDto.ActorIds.Select(actorId => new MovieActor
                {
                    MovieId = id,
                    ActorId = actorId
                }).ToList();
            }

            await _context.SaveChangesAsync();

            return await GetByIdAsync(id);
        }

        public async Task DeleteAsync(int id)
        {
            var movie = await _context.Movies
                .FirstOrDefaultAsync(m => m.Id == id && m.IsActive);

            if (movie == null)
                throw new KeyNotFoundException($"Movie with ID {id} not found.");

            // Check if movie has any orders
            var hasOrders = await _context.Orders.AnyAsync(o => o.MovieId == id);
            if (hasOrders)
                throw new InvalidOperationException("Cannot delete movie with existing orders.");

            // Soft delete
            movie.IsActive = false;
            await _context.SaveChangesAsync();
        }
    }
} 