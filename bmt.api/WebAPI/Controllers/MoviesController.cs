using Core.Application.Interfaces;
using Core.Shared.DTOs.Movie;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class MovieController : ControllerBase
{
    private readonly IMovieService<MovieRes> _service;

    public MovieController(IMovieService<MovieRes> service)
    {
        _service = service;
    }

    /// <summary>
    /// read movie by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id}")]
    [ProducesResponseType(200, Type = typeof(MovieRes))]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _service.GetMovieById(id);

        if (result == null || !result.IsSuccess)
        {
            return NotFound();
        }

        return Ok(result.Data);
    }
}
