using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Business.Abstract;
using Core.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Attributes;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SocketsController : ControllerBase
    {
        private readonly IRoomService _roomService;

        public SocketsController(IRoomService roomService)
        {
            _roomService = roomService;
        }

        [Route("rooms")]
        [HttpGet]
        public IActionResult GetRooms()
        {
            var result = _roomService.GetRooms();
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }
            return Ok(result);
        }

        [Route("rooms/room")]
        [HttpGet]
        public IActionResult GetRoomById(string roomId)
        {
            var result = _roomService.GetRoomById(roomId);
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }
            return Ok(result);
        }

        [Route("rooms/user")]
        [HttpGet]
        [Authorize]
        public IActionResult GetRoomsWithUser()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity != null)
            {
                IEnumerable<Claim> claims = identity.Claims;
                string userEmail = claims.First(x => x.Type == ClaimTypes.Email).Value;
                var result = _roomService.GetRoomsWithUser(userEmail);
                if (!result.Success)
                {
                    return BadRequest(result.Message);
                }
                return Ok(result);
            }

            return BadRequest();
        }

        [Route("rooms")]
        [DMAuthorize]
        [HttpDelete]
        public IActionResult DeleteRoom(DeleteRoomDto deleteRoomDto)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity != null)
            {
                IEnumerable<Claim> claims = identity.Claims;
                string userEmail = claims.First(x => x.Type == ClaimTypes.Email).Value;
                var result = _roomService.DeleteRoom(userEmail, deleteRoomDto.RoomId);
                if (!result.Success)
                {
                    return BadRequest(result.Message);
                }
                return Ok(result);
            }

            return BadRequest();
        }


        [Route("rooms")]
        [HttpPost]
        [DMAuthorize]
        public IActionResult CreateRoom(CreateRoomDto createRoomDto)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity != null)
            {
                IEnumerable<Claim> claims = identity.Claims;
                string userEmail = claims.First(x => x.Type == ClaimTypes.Email).Value;
                var result = _roomService.CreateRoom(userEmail, createRoomDto.RoomName, createRoomDto.Description);
                if (!result.Success)
                {
                    return BadRequest(result.Message);
                }
                else
                {
                    return Ok(result);
                }
            }

                return BadRequest();
        }
    }
}