using System.Collections.Generic;
using System.Threading.Tasks;
using MovieStore.Api.DTOs.Movie;

namespace MovieStore.Api.Services
{
    public interface IMovieService
    {
        Task<List<MovieDto>> GetAllAsync();
        Task<MovieDto> GetByIdAsync(int id);
        Task<MovieDto> CreateAsync(CreateMovieDto createMovieDto);
        Task<MovieDto> UpdateAsync(int id, UpdateMovieDto updateMovieDto);
        Task DeleteAsync(int id);
    }
} 