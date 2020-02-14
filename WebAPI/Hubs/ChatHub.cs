using Business.Abstract;
using Core.Abstract;
using Core.Entities.Models;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

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

        public void SendToRoom(string userId,string roomId, AbstractMessageBaseDto message)
        {
        }

        public void Join(string userId,string roomId)
        {

        }

        private void Leave(string userId, string roomId)
        {

        }

        public void CreateRoom(string userId)
        {

        }

        public void DeleteRoom(string userId)
        {

        }

        public IEnumerable<Message> GetMessageHistory(string roomId)
        {
            return null;
        }

        public IEnumerable<Room> GetRooms()
        {
            return null;
        }

        public IEnumerable<User> GetUsers(string roomId)
        {
            return null;
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
