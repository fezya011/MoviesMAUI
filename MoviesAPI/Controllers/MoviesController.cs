using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MoviesAPI.CQRS.Commands;
using MoviesAPI.CQRS.DTO;
using MyMediator.Interfaces;

namespace MoviesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public MoviesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("AddMovie")]
        public async Task<ActionResult> AddMovie(MovieDTO movieDTO)
        {         
            var command = new AddMovieCommand
            {
                Movie = movieDTO,
            };

            try
            {
                await _mediator.SendAsync(command);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok();
        }
    }
}
