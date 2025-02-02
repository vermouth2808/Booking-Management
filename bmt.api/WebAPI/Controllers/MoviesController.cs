using Core.Application.Interfaces;
using Core.Shared.DTOs.Movie;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
[Authorize] 
public class MovieController : ControllerBase
{
    private readonly IMovieService<MovieRes> _serviceMovie;

    public MovieController(IMovieService<MovieRes> serviceMovie)
    {
        _serviceMovie = serviceMovie;
    }

    [HttpGet("{id}")]
    [ProducesResponseType(200, Type = typeof(MovieRes))]
    [Authorize(Roles = "client")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _serviceMovie.GetMovieById(id);

        if (result == null || !result.IsSuccess)
        {
            return NotFound();
        }

        return Ok(result.Data);
    }
}
