using Core.Dtos.RoomDtos;
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
        IResult CreateRoom(CreateRoomDto createRoomDto);
        IResult DeleteRoom(DeleteRoomDto deleteRoomDto);
        IResult AddUserToRoom(AddUserToRoomDto userJoinRoomDto);
        IResult RemoveUserFromRoom(RemoveUserFromRoomDto removeUserFromRoomDto);
        IResult RemoveUserFromAllRooms(string userEmail);
    }
}
