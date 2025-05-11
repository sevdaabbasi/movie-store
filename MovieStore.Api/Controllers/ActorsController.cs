using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieStore.Api.DTOs.Actor;
using MovieStore.Api.Services;

namespace MovieStore.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ActorsController : ControllerBase
    {
        private readonly IActorService _actorService;

        public ActorsController(IActorService actorService)
        {
            _actorService = actorService;
        }

        [HttpGet]
        public async Task<ActionResult<List<ActorDto>>> GetAll()
        {
            var actors = await _actorService.GetAllAsync();
            return Ok(actors);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ActorDto>> GetById(int id)
        {
            var actor = await _actorService.GetByIdAsync(id);
            return Ok(actor);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult<ActorDto>> Create(CreateActorDto createActorDto)
        {
            var actor = await _actorService.CreateAsync(createActorDto);
            return CreatedAtAction(nameof(GetById), new { id = actor.Id }, actor);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<ActionResult<ActorDto>> Update(int id, UpdateActorDto updateActorDto)
        {
            var actor = await _actorService.UpdateAsync(id, updateActorDto);
            return Ok(actor);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            await _actorService.DeleteAsync(id);
            return NoContent();
        }
    }
} 