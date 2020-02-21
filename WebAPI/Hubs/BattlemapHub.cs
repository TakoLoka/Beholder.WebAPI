﻿using Business.Abstract;
using Core.Abstract;
using Core.Entities.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using WebAPI.Attributes;
using WebAPI.Messages;

namespace WebAPI.Hubs
{
    public class BattlemapHub : Hub
    {
        private readonly IRoomService _roomService;

        public BattlemapHub(IRoomService roomService)
        {
            _roomService = roomService;
        }

        [DMAuthorize]
        public async Task CreateRoom()
        {
            var identity = Context.User.Identity as ClaimsIdentity;
            if (identity != null)
            {
                IEnumerable<Claim> claims = identity.Claims;
                string userEmail = claims.First(x => x.Type == ClaimTypes.Email).Value;
                var result = _roomService.CreateRoom(userEmail);
                if (!result.Success)
                {
                    await Clients.Caller.SendAsync("CREATE_ROOM_ERROR", result.Message);
                }
                else
                {
                    await Groups.AddToGroupAsync(Context.ConnectionId, result.Message);
                    await Clients.Caller.SendAsync("CREATE_ROOM", result.Message);
                }
            }
            else
            {

                await Clients.Caller.SendAsync("BAD_REQUEST");
            }
        }

        [Authorize]
        public async Task SendMessage(string roomName, string message)
        {
            var identity = Context.User.Identity as ClaimsIdentity;
            IEnumerable<Claim> claims = identity.Claims;
            string userEmail = claims.First(x => x.Type == ClaimTypes.Email).Value;
            await Clients.Group(roomName).SendAsync("MESSAGE", $"{userEmail}: {message}");
        }

        [Authorize]
        public async Task JoinRoom(string roomName)
        {
            var identity = Context.User.Identity as ClaimsIdentity;
            if (identity.IsAuthenticated)
            {
                IEnumerable<Claim> claims = identity.Claims;
                string userEmail = claims.First(x => x.Type == ClaimTypes.Email).Value;
                var result = _roomService.AddUserToRoom(userEmail, roomName);
                if (!result.Success)
                {
                    await Clients.Caller.SendAsync("USER_CONNECTED_ROOM_ERROR", result.Message);
                }
                else
                {
                    await Groups.AddToGroupAsync(Context.ConnectionId, roomName);
                    await Clients.Group(roomName).SendAsync("USER_CONNECTED_ROOM", result.Message);
                }
            }
            else
            {
                await Clients.Caller.SendAsync("BAD_REQUEST");
            }
        }
        [Authorize]
        public async Task LeaveRoom(string roomName)
        {
            var identity = Context.User.Identity as ClaimsIdentity;
            if (identity.IsAuthenticated)
            {
                IEnumerable<Claim> claims = identity.Claims;
                string userEmail = claims.First(x => x.Type == ClaimTypes.Email).Value;
                var result = _roomService.RemoveUserFromRoom(userEmail, roomName);
                if (!result.Success)
                {
                    await Clients.Caller.SendAsync("USER_DISCONNECTED_ROOM_ERROR", result.Message);
                }
                else
                {
                    await Groups.RemoveFromGroupAsync(Context.ConnectionId, roomName);
                    await Clients.Group(roomName).SendAsync("USER_DISCONNECTED_ROOM", result.Message);
                }
            }
            else
            {
                await Clients.Caller.SendAsync("BAD_REQUEST");
            }
        }

        #region OnConnected/OnDisconnected
        public override async Task OnConnectedAsync()
        {
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var identity = Context.User.Identity as ClaimsIdentity;
            List<Room> rooms = _roomService.GetRoomsWithUser(identity.Claims.First(x => x.Type == ClaimTypes.Email).Value).Data;
            foreach (var room in rooms)
            {
                await Clients.Group(room.RoomName.ToString()).SendAsync("USER_DISCONNECTED", "User Disconnected");
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, room.RoomName.ToString());
            }
            _roomService.RemoveUserFromAllRooms(identity.Claims.First(x => x.Type == ClaimTypes.Email).Value);
            await base.OnDisconnectedAsync(exception);
        }
        #endregion
    }
}
