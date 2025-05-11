using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MovieStore.Api.Data;
using MovieStore.Api.DTOs.Director;
using MovieStore.Api.Entities;

namespace MovieStore.Api.Services
{
    public class DirectorService : IDirectorService
    {
        private readonly MovieStoreDbContext _context;
        private readonly IMapper _mapper;

        public DirectorService(MovieStoreDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<DirectorDto>> GetAllAsync()
        {
            var directors = await _context.Directors
                .Include(d => d.Movies)
                .ToListAsync();

            return _mapper.Map<List<DirectorDto>>(directors);
        }

        public async Task<DirectorDto> GetByIdAsync(int id)
        {
            var director = await _context.Directors
                .Include(d => d.Movies)
                .FirstOrDefaultAsync(d => d.Id == id);

            if (director == null)
                throw new KeyNotFoundException($"Director with ID {id} not found.");

            return _mapper.Map<DirectorDto>(director);
        }

        public async Task<DirectorDto> CreateAsync(CreateDirectorDto createDirectorDto)
        {
            var director = _mapper.Map<Director>(createDirectorDto);
            _context.Directors.Add(director);
            await _context.SaveChangesAsync();

            return await GetByIdAsync(director.Id);
        }

        public async Task<DirectorDto> UpdateAsync(int id, UpdateDirectorDto updateDirectorDto)
        {
            var director = await _context.Directors.FindAsync(id);

            if (director == null)
                throw new KeyNotFoundException($"Director with ID {id} not found.");

            _mapper.Map(updateDirectorDto, director);
            await _context.SaveChangesAsync();

            return await GetByIdAsync(id);
        }

        public async Task DeleteAsync(int id)
        {
            var director = await _context.Directors
                .Include(d => d.Movies)
                .FirstOrDefaultAsync(d => d.Id == id);

            if (director == null)
                throw new KeyNotFoundException($"Director with ID {id} not found.");

            // Check if director has any movies
            if (director.Movies.Any())
                throw new InvalidOperationException("Cannot delete director with existing movies.");

            _context.Directors.Remove(director);
            await _context.SaveChangesAsync();
        }
    }
} 