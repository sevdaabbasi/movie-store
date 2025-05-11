using System.Collections.Generic;
using System.Threading.Tasks;
using MovieStore.Api.DTOs.Director;

namespace MovieStore.Api.Services
{
    public interface IDirectorService
    {
        Task<List<DirectorDto>> GetAllAsync();
        Task<DirectorDto> GetByIdAsync(int id);
        Task<DirectorDto> CreateAsync(CreateDirectorDto createDirectorDto);
        Task<DirectorDto> UpdateAsync(int id, UpdateDirectorDto updateDirectorDto);
        Task DeleteAsync(int id);
    }
} 