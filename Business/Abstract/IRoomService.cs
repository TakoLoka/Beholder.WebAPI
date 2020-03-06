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
        IDataResult<List<Room>> GetRoomsWithUser(string userEmail);
        IDataResult<Room> GetRoomById(string roomId);
        IDataResult<Room> GetRoomByName(string roomName);
        IResult CreateRoom(string creatorEmail, string roomName, string description);
        IResult DeleteRoom(string creatorEmail, string roomId);
        IResult AddUserToRoom(string email, string roomId);
        IResult RemoveUserFromRoom(string email, string roomId);
        IResult RemoveUserFromAllRooms(string value);
    }
}
