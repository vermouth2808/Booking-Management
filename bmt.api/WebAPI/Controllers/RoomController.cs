using Core.Application.Interfaces;
using Core.Shared.DTOs.Request.Room;
using Core.Shared.DTOs.Response.Room;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class RoomsController : ControllerBase
{
    private readonly IRoomService<RoomRes> _serviceRoom;

    public RoomsController(IRoomService<RoomRes> serviceRoom)
    {
        _serviceRoom = serviceRoom;
    }
 /*   /// <summary>
    /// search Room
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    [HttpPost("SearchRoom")]
    [ProducesResponseType(200, Type = typeof(IEnumerable<RoomSearchRes>))]
    [Authorize(Roles = "client,admin")]
    public async Task<IActionResult> SearchRoom(SearchRoomReq req)
    {
        var result = await _serviceRoom.SearchRoom(req);
        if (result is null || !result.IsSuccess)
        {
            return NotFound();
        }
        return Ok(result.Data);
    }*/
    /// <summary>
    /// Create a Room
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    [HttpPost("CreateRoom")]
    [ProducesResponseType(200, Type = typeof(bool))]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> CreateRoom(CreateRoomReq req)
    {
        if (!int.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out int userId))
        {
            return Unauthorized("Invalid User ID in token.");
        }

        var result = await _serviceRoom.CreateRoom(req, userId);
        if (result is null || !result.IsSuccess)
        {
            return NotFound();
        }
        return Ok(result.Data);
    }

    /// <summary>
    /// Update a Room
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    [HttpPut("UpdateRoom")]
    [ProducesResponseType(200, Type = typeof(bool))]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> UpdateRoom(UpdateRoomReq req)
    {
        if (!int.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out int userId))
        {
            return Unauthorized("Invalid User ID in token.");
        }

        var result = await _serviceRoom.UpdateRoom(req, userId);
        if (result is null || !result.IsSuccess)
        {
            return NotFound();
        }
        return Ok(result.Data);
    }
    /// <summary>
    /// read by id Room
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("GetById/{id}")]
    [ProducesResponseType(200, Type = typeof(RoomRes))]
    [Authorize(Roles = "client,admin")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _serviceRoom.GetRoomById(id);

        if (result == null || !result.IsSuccess)
        {
            return NotFound();
        }

        return Ok(result.Data);
    }
    /// <summary>
    /// Remove a Room
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete("{id}")]
    [ProducesResponseType(200, Type = typeof(bool))]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> DeleteRoom(int id)
    {
        if (!int.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out int userId))
        {
            return Unauthorized("Invalid User ID in token.");
        }
        var result = await _serviceRoom.DeleteRoom(id, userId);

        if (result == null || !result.IsSuccess)
        {
            return NotFound();
        }

        return Ok(result.Data);
    }

}
