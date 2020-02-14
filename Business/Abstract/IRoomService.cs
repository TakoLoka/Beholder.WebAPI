using Core.Entities.Models;
using Core.Utilities.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Abstract
{
    public interface IRoomService
    {
        IDataResult<List<Room>> GetRooms();
        IDataResult<Room> GetRoomByName(string roomName);
        IResult CreateRoom(string creatorEmail);
        IResult DeleteRoom(string creatorEmail, string roomName);
        IResult AddUserToRoom(string email, string roomName);
        IResult RemoveUserFromRoom(string email, string roomName);
    }
}
