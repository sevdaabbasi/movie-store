using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MovieStore.Api.Data;
using MovieStore.Api.DTOs.Actor;
using MovieStore.Api.Entities;

namespace MovieStore.Api.Services
{
    public class ActorService : IActorService
    {
        private readonly MovieStoreDbContext _context;
        private readonly IMapper _mapper;

        public ActorService(MovieStoreDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<ActorDto>> GetAllAsync()
        {
            var actors = await _context.Actors
                .Include(a => a.MovieActors)
                    .ThenInclude(ma => ma.Movie)
                .ToListAsync();

            return _mapper.Map<List<ActorDto>>(actors);
        }

        public async Task<ActorDto> GetByIdAsync(int id)
        {
            var actor = await _context.Actors
                .Include(a => a.MovieActors)
                    .ThenInclude(ma => ma.Movie)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (actor == null)
                throw new KeyNotFoundException($"Actor with ID {id} not found.");

            return _mapper.Map<ActorDto>(actor);
        }

        public async Task<ActorDto> CreateAsync(CreateActorDto createActorDto)
        {
            var actor = _mapper.Map<Actor>(createActorDto);
            _context.Actors.Add(actor);
            await _context.SaveChangesAsync();

            return await GetByIdAsync(actor.Id);
        }

        public async Task<ActorDto> UpdateAsync(int id, UpdateActorDto updateActorDto)
        {
            var actor = await _context.Actors.FindAsync(id);

            if (actor == null)
                throw new KeyNotFoundException($"Actor with ID {id} not found.");

            _mapper.Map(updateActorDto, actor);
            await _context.SaveChangesAsync();

            return await GetByIdAsync(id);
        }

        public async Task DeleteAsync(int id)
        {
            var actor = await _context.Actors
                .Include(a => a.MovieActors)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (actor == null)
                throw new KeyNotFoundException($"Actor with ID {id} not found.");

            // Check if actor has any movies
            if (actor.MovieActors.Any())
                throw new InvalidOperationException("Cannot delete actor with existing movies.");

            _context.Actors.Remove(actor);
            await _context.SaveChangesAsync();
        }
    }
} 