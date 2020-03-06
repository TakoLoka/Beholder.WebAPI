using Business.Abstract;
using Business.Constants;
using Core.Entities.Models;
using Core.Utilities.Results;
using DataAccess.Abstract;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Concrete
{
    public class RoomManager : IRoomService
    {
        private readonly IUserService _userService;
        private readonly IRoomDal _roomDal;

        public RoomManager(IUserService userService, IRoomDal roomDal)
        {
            _userService = userService;
            _roomDal = roomDal;
        }

        public IResult AddUserToRoom(string email, string roomId)
        {
            if(Guid.TryParse(roomId, out var parsedRoomId))
            {
                var userToAdd = _userService.GetByMail(email).Data;
                if(userToAdd == null)
                {
                    return new ErrorResult(Messages.UserNotFound);
                }
                var roomToAdd = _roomDal.GetOne(room => room.RoomId == parsedRoomId);
                if (roomToAdd == null)
                {
                    return new ErrorResult(Messages.RoomMessages.RoomDoesNotExist);
                }

                if (roomToAdd.Users == null)
                    roomToAdd.Users = new List<User>();

                if (roomToAdd.Users.Find(user => user.Email == userToAdd.Email) != null)
                {
                    return new ErrorResult(Messages.RoomMessages.UserAlreadyExists);
                }

                roomToAdd.Users.Add(userToAdd);
                _roomDal.Update(roomToAdd.Id.ToString(), roomToAdd);

                return new SuccessResult(Messages.RoomMessages.UserAddedToRoom(userToAdd, roomToAdd));
            }

            return new ErrorResult(Messages.GuidError);
        }

        public IResult CreateRoom(string creatorEmail, string roomName, string description)
        {
            var assignedId = Guid.NewGuid();
            var creator = _userService.GetByMail(creatorEmail).Data;
            _roomDal.Create(new Room
            {
                RoomId = assignedId,
                Creator = creator,
                RoomName = roomName,
                Description = description
            });

            return new SuccessResult(assignedId.ToString());
        }

        public IResult DeleteRoom(string creatorEmail, string roomId)
        {
            if (Guid.TryParse(roomId, out var assignedId))
            {
                var roomToDelete = _roomDal.GetOne(room => room.RoomId == assignedId);
                if (creatorEmail == roomToDelete.Creator.Email)
                {
                    _roomDal.Delete(roomToDelete);

                    return new SuccessResult(Messages.RoomMessages.RoomDeleted(roomToDelete));
                }

                return new ErrorResult(Messages.RoomMessages.UserIsNotTheCreator);
            }

            return new ErrorResult(Messages.GuidError);
        }

        public IDataResult<Room> GetRoomById(string roomId)
        {
            var assignedId = new Guid(roomId);
            return new SuccessDataResult<Room>(_roomDal.GetOne(room => room.RoomId == assignedId));
        }

        public IDataResult<List<Room>> GetRooms()
        {
            return new SuccessDataResult<List<Room>>(_roomDal.GetList());
        }

        public IDataResult<List<Room>> GetRoomsWithUser(string userEmail)
        {
            var user = _userService.GetByMail(userEmail).Data;
            return new SuccessDataResult<List<Room>>(_roomDal.GetList(room => room.Users.Contains(user)));
        }

        public IResult RemoveUserFromRoom(string email, string roomId)
        {
            if (Guid.TryParse(roomId, out var assignedId))
            {

                var removeRoom = _roomDal.GetOne(room => room.RoomId == assignedId);

                if (removeRoom == null)
                {
                    return new ErrorResult(Messages.RoomMessages.RoomDoesNotExist);
                }

                var userToRemove = removeRoom.Users.Find(x => x.Email == email);

                if (removeRoom.Users.Find(user => user.Email == userToRemove.Email) == null)
                {
                    return new ErrorResult(Messages.RoomMessages.UserIsNotInThisRoom);
                }

                //if (userToRemove.Email == removeRoom.Creator.Email)
                //{
                //    return DeleteRoom(userToRemove.Email, removeRoom.RoomId.ToString());
                //}

                removeRoom.Users.Remove(userToRemove);
                _roomDal.Update(removeRoom.Id.ToString(), removeRoom);

                return new SuccessResult(Messages.RoomMessages.UserRemovedFromRoom(userToRemove, removeRoom));
            }

            return new ErrorResult(Messages.GuidError);
        }

        public IResult RemoveUserFromAllRooms(string userEmail)
        {
            var userToRemove = _userService.GetByMail(userEmail).Data;
            List<Room> rooms = _roomDal.GetList(x => x.Users.Contains(userToRemove));
            if(rooms != null && rooms.Count >= 0)
            {
                foreach (var room in rooms)
                {
                    RemoveUserFromRoom(userEmail, room.RoomId.ToString());
                }
            }

            return new SuccessResult(Messages.RoomMessages.UserRemovedFromAllRooms);
        }

        public IDataResult<Room> GetRoomByName(string roomName)
        {
            return new SuccessDataResult<Room>(_roomDal.GetOne(room => room.RoomName == roomName));
        }
    }
}
