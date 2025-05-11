using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieStore.Api.DTOs.Director;
using MovieStore.Api.Services;

namespace MovieStore.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DirectorsController : ControllerBase
    {
        private readonly IDirectorService _directorService;

        public DirectorsController(IDirectorService directorService)
        {
            _directorService = directorService;
        }

        [HttpGet]
        public async Task<ActionResult<List<DirectorDto>>> GetAll()
        {
            var directors = await _directorService.GetAllAsync();
            return Ok(directors);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<DirectorDto>> GetById(int id)
        {
            var director = await _directorService.GetByIdAsync(id);
            return Ok(director);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult<DirectorDto>> Create(CreateDirectorDto createDirectorDto)
        {
            var director = await _directorService.CreateAsync(createDirectorDto);
            return CreatedAtAction(nameof(GetById), new { id = director.Id }, director);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<ActionResult<DirectorDto>> Update(int id, UpdateDirectorDto updateDirectorDto)
        {
            var director = await _directorService.UpdateAsync(id, updateDirectorDto);
            return Ok(director);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            await _directorService.DeleteAsync(id);
            return NoContent();
        }
    }
} 