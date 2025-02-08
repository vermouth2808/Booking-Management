using Core.Application.Interfaces;
using Core.Shared.DTOs.Request.Movie;
using Core.Shared.DTOs.Response.Movie;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
[Authorize] 
public class MoviesController : ControllerBase
{
    private readonly IMovieService<MovieRes> _serviceMovie;

    public MoviesController(IMovieService<MovieRes> serviceMovie)
    {
        _serviceMovie = serviceMovie;
    }
    /// <summary>
    /// search movie
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    [HttpPost("SearchMovie")]
    [ProducesResponseType(200,Type=typeof(IEnumerable<SearchMovieReq>))]
    [Authorize(Roles ="client,admin")]
    public async Task<IActionResult> SearchMovie (SearchMovieReq req)
    {
        var result = await _serviceMovie.SearchMovie(req);
        if (result is null ||!result.IsSuccess)
        {
            return NotFound();
        }
        return Ok(result.Data);
    }
    /// <summary>
    /// Create a movie
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    [HttpPost("CreateMovie")]
    [ProducesResponseType(200, Type = typeof(bool))]
    [Authorize(Roles = "client,admin")]
    public async Task<IActionResult> CreateMovie(CreateMovieReq req)
    {
        if (!int.TryParse(User.FindFirst("UserId")?.Value, out int userId))
        {
            return Unauthorized("Invalid User ID in token.");
        }
        var result = await _serviceMovie.CreateMovie(req, userId);
        if (result is null || !result.IsSuccess)
        {
            return NotFound();
        }
        return Ok(result.Data);
    }
    /// <summary>
    /// read by id movie
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("GetById/{id}")]
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
