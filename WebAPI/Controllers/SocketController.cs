﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Business.Abstract;
using Business.Constants;
using Core.Dtos;
using Core.Entities.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Attributes;
using WebAPI.Hubs;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SocketController : ControllerBase
    {
        private readonly IRoomService _roomService;

        public SocketController(IRoomService roomService)
        {
            _roomService = roomService;
        }

        [Route("rooms")]
        [HttpGet]
        public IActionResult GetRooms()
        {
            return Ok(_roomService.GetRooms());
        }

        [Route("rooms")]
        [DMAuthorize]
        [HttpPost]
        public IActionResult CreateRoom()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity != null)
            {
                IEnumerable<Claim> claims = identity.Claims;
                string userEmail = claims.First(x => x.Type == ClaimTypes.Email).Value;
                var result = _roomService.CreateRoom(userEmail);
                if (!result.Success)
                {
                    return BadRequest(result.Message);
                }
                return CreatedAtAction(nameof(CreateRoom), result);
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
                var result = _roomService.DeleteRoom(userEmail, deleteRoomDto.RoomName);
                if (!result.Success)
                {
                    return BadRequest(result.Message);
                }
                return Ok(result.Message);
            }

            return BadRequest();
        }

        [Route("rooms/join")]
        [Authorize]
        [HttpPatch]
        public IActionResult JoinRoom(UserJoinRoomDto userJoinRoomDto)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity != null)
            {
                IEnumerable<Claim> claims = identity.Claims;
                string userEmail = claims.First(x => x.Type == ClaimTypes.Email).Value;
                var result = _roomService.AddUserToRoom(userEmail, userJoinRoomDto.RoomName);
                if (!result.Success)
                {
                    return BadRequest(result.Message);
                }
                return Ok(result);
            }

            return BadRequest();
        }

        [Route("rooms/remove")]
        [Authorize]
        [HttpPatch]
        public IActionResult RemoveUserFromRoom(RemoveUserFromRoomDto removeUserFromRoomDto)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity != null)
            {
                IEnumerable<Claim> claims = identity.Claims;
                string userEmail = claims.First(x => x.Type == ClaimTypes.Email).Value;
                var result = _roomService.RemoveUserFromRoom(userEmail, removeUserFromRoomDto.RoomName);
                if (!result.Success)
                {
                    return BadRequest(result.Message);
                }
                return Ok(result);
            }

            return BadRequest();
        }
    }
}