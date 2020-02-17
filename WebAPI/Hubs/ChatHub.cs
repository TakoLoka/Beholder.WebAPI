using Business.Abstract;
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
    public class ChatHub: Hub
    {
        private readonly IRoomService _roomService;

        public ChatHub(IRoomService roomService)
        {
            _roomService = roomService;
        }
        [Authorize]
        public async Task Join(string roomName)
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
                    await Clients.Group(roomName).SendAsync("USER_CONNECTED_ROOM", result);
                }
            }
            else
            {
                await Clients.Caller.SendAsync("BAD_REQUEST");
            }
        }
        [Authorize]
        private async Task Leave(string roomName)
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
                    await Clients.Group(roomName).SendAsync("USER_DISCONNECTED_ROOM", result);
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
            await Clients.Caller.SendAsync("USER_CONNECTED", CancellationToken.None);
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            await Clients.Caller.SendAsync("USER_DISCONNECTED", CancellationToken.None);
            await base.OnConnectedAsync();
        }
        #endregion
    }
}
