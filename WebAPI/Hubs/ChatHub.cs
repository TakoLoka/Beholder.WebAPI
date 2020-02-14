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
using System.Threading.Tasks;
using WebAPI.Attributes;
using WebAPI.Messages;

namespace WebAPI.Hubs
{
    public class ChatHub: Hub
    {
        private readonly IUserService _userService;
        private readonly IRoomService _roomService;

        public ChatHub(IUserService userService, IRoomService roomService)
        {
            _userService = userService;
            _roomService = roomService;
        }

        //[Authorize]
        //public async Task CharacterUpdated(string roomName, UpdateCharacterMessage updateCharacterMessage)
        //{
        //    _roomService.GetRoomByName(roomName);
        //    var identity = Context.User.Identity as ClaimsIdentity;
        //    if (identity != null)
        //    {
        //        IEnumerable<Claim> claims = identity.Claims;
        //        string userEmail = claims.First(x => x.Type == ClaimTypes.Email).Value;
        //        var result = _roomService.UpdateCharacter(userEmail);
        //        if (!result.Success)
        //        {
        //            await Clients.Group(roomName).SendAsync("CHARACTER_UPDATED_ERROR", result.Message);
        //        }
        //        await Clients.Group(roomName).SendAsync("CHARACTER_UPDATED",result);
        //    }

        //    await Clients.Group(roomName).SendAsync("BAD_REQUEST");
        //}
        [Authorize]
        public async Task Join(string roomName)
        {
            _roomService.GetRoomByName(roomName);
            var identity = Context.User.Identity as ClaimsIdentity;
            if (identity != null)
            {
                IEnumerable<Claim> claims = identity.Claims;
                string userEmail = claims.First(x => x.Type == ClaimTypes.Email).Value;
                var result = _roomService.AddUserToRoom(userEmail, roomName);
                if (!result.Success)
                {
                    await Clients.Group(roomName).SendAsync("USER_CONNECTED_ERROR", result.Message);
                }
                await Clients.Group(roomName).SendAsync("USER_CONNECTED", result);
            }

            await Clients.Group(roomName).SendAsync("BAD_REQUEST");
        }
        [Authorize]
        private async Task Leave(string roomName)
        {
            _roomService.GetRoomByName(roomName);
            var identity = Context.User.Identity as ClaimsIdentity;
            if (identity != null)
            {
                IEnumerable<Claim> claims = identity.Claims;
                string userEmail = claims.First(x => x.Type == ClaimTypes.Email).Value;
                var result = _roomService.RemoveUserFromRoom(userEmail, roomName);
                if (!result.Success)
                {
                    await Clients.Group(roomName).SendAsync("USER_DISCONNECTED_ERROR", result.Message);
                }
                await Clients.Group(roomName).SendAsync("USER_DISCONNECTED", result);
            }

            await Clients.Group(roomName).SendAsync("BAD_REQUEST");
        }

        #region OnConnected/OnDisconnected
        public override Task OnConnectedAsync()
        {
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            return base.OnDisconnectedAsync(exception);
        }
        #endregion
    }
}
