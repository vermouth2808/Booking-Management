using Core.Application.Interfaces;
using Core.Shared.DTOs.Request.Movie;
using Core.Shared.DTOs.Response.Movie;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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
    [ProducesResponseType(200, Type = typeof(IEnumerable<MovieSearchRes>))]
    [Authorize(Roles = "client,admin")]
    public async Task<IActionResult> SearchMovie(SearchMovieReq req)
    {
        var result = await _serviceMovie.SearchMovie(req);
        if (result is null || !result.IsSuccess)
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
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> CreateMovie(CreateMovieReq req)
    {
        if (!int.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out int userId))
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
    /// Update a movie
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    [HttpPut("UpdateMovie")]
    [ProducesResponseType(200, Type = typeof(bool))]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> UpdateMovie(UpdateMovieReq req)
    {
        if (!int.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out int userId))
        {
            return Unauthorized("Invalid User ID in token.");
        }

        var result = await _serviceMovie.UpdateMovie(req, userId);
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
    [Authorize(Roles = "client,admin")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _serviceMovie.GetMovieById(id);

        if (result == null || !result.IsSuccess)
        {
            return NotFound();
        }

        return Ok(result.Data);
    }
    /// <summary>
    /// Remove a movie
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete("{id}")]
    [ProducesResponseType(200, Type = typeof(bool))]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> DeleteMovie(int id)
    {
        if (!int.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out int userId))
        {
            return Unauthorized("Invalid User ID in token.");
        }
        var result = await _serviceMovie.DeleteMovie(id, userId);

        if (result == null || !result.IsSuccess)
        {
            return NotFound();
        }

        return Ok(result.Data);
    }

}
