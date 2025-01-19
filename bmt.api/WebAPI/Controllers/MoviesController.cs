
using Core.Services;
using Microsoft.AspNetCore.Mvc;
using Core.Repositories.Model;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MovieController : ControllerBase
    {
        private readonly IMovieService<Movie> _service;
        public MovieController(IMovieService<Movie> service)
        {
            _service = service;
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var result = _service.GetMovieById(id);

            if(result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        [HttpPost("add")]
        public IActionResult Add(Movie movie)
        {
            var result = _service.AddMovie(movie);

            if(result == true)
            {
                return Ok();
            }

            return BadRequest();
        }

        //[HttpGet("Read_All")]
        //public async Task<IActionResult> GetAllMovies()
        //{
        //    var result = await _movieService.Read_All();

        //    if (result.IsSuccess)
        //    {
        //        return Ok(result.Data);
        //    }

        //    return NotFound(result.Message); // Trả về 404 nếu không tìm thấy phim
        //}


        //[HttpPost("Create")]
        //public async Task<IActionResult> Create(Movie_CreateReq req)
        //{
        //    if (req == null)
        //    {
        //        return BadRequest("Invalid movie data.");
        //    }

        //    var result = await _movieService.Create(req, 1);

        //    if (result.IsSuccess)
        //    {
        //        // Trả về chỉ thông báo mà không có data
        //        return CreatedAtAction(nameof(Create), new { message = result.Message });
        //    }

        //    return BadRequest(result.Message);
        //}

    }
}
