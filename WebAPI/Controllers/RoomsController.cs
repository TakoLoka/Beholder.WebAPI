using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Business.Abstract;
using Core.Dtos.RoomDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Attributes;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomsController : ControllerBase
    {
        private readonly IRoomService _roomService;

        public RoomsController(IRoomService roomService)
        {
            _roomService = roomService;
        }

        [Route("")]
        [HttpGet]
        public IActionResult GetRooms()
        {
            var result = _roomService.GetRooms();
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }

            List<RoomRepresentationDto> allRooms = new List<RoomRepresentationDto>();
            foreach (var room in result.Data)
            {
                allRooms.Add(new RoomRepresentationDto(room));
            }
            return Ok(allRooms);
        }

        [Route("room")]
        [HttpGet]
        public IActionResult GetRoomById(string roomId)
        {
            var result = _roomService.GetRoomById(roomId);
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }
            return Ok(new RoomRepresentationDto(result.Data));
        }

        [Route("user")]
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
                List<RoomRepresentationDto> roomsWithUser = new List<RoomRepresentationDto>();
                foreach (var room in result.Data)
                {
                    roomsWithUser.Add(new RoomRepresentationDto(room));
                }
                return Ok(roomsWithUser);
            }

            return BadRequest();
        }

        [Route("")]
        [DMAuthorize]
        [HttpDelete]
        public IActionResult DeleteRoom(DeleteRoomDto deleteRoomDto)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity != null)
            {
                IEnumerable<Claim> claims = identity.Claims;
                string userEmail = claims.First(x => x.Type == ClaimTypes.Email).Value;
                var result = _roomService.DeleteRoom(deleteRoomDto);
                if (!result.Success)
                {
                    return BadRequest(result.Message);
                }
                return Ok(result.Message);
            }

            return BadRequest();
        }


        [Route("")]
        [HttpPost]
        [DMAuthorize]
        public IActionResult CreateRoom(CreateRoomDto createRoomDto)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity != null)
            {
                IEnumerable<Claim> claims = identity.Claims;
                string userEmail = claims.First(x => x.Type == ClaimTypes.Email).Value;
                var result = _roomService.CreateRoom(createRoomDto);
                if (!result.Success)
                {
                    return BadRequest(result.Message);
                }
                else
                {
                    return Ok(result.Message);
                }
            }

                return BadRequest();
        }
    }
}