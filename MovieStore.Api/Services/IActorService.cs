using System.Collections.Generic;
using System.Threading.Tasks;
using MovieStore.Api.DTOs.Actor;

namespace MovieStore.Api.Services
{
    public interface IActorService
    {
        Task<List<ActorDto>> GetAllAsync();
        Task<ActorDto> GetByIdAsync(int id);
        Task<ActorDto> CreateAsync(CreateActorDto createActorDto);
        Task<ActorDto> UpdateAsync(int id, UpdateActorDto updateActorDto);
        Task DeleteAsync(int id);
    }
} 