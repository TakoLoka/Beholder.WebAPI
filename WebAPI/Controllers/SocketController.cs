using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Business.Abstract;
using Business.Constants;
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
        [HttpPost]
        public IActionResult CreateRoom()
        {
            return Ok(_roomService.CreateRoom());
        }

        [Route("rooms")]
        [HttpDelete]
        public IActionResult DeleteRoom(Room room)
        {
            return Ok(_roomService.DeleteRoom(room.RoomName.ToString()));
        }
    }
}