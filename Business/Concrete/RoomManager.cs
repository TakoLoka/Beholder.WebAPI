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

        public IResult AddUserToRoom(string email, string roomName)
        {
            if(Guid.TryParse(roomName, out var parsedRoomName))
            {
                var userToAdd = _userService.GetByMail(email).Data;
                if(userToAdd == null)
                {
                    return new ErrorResult(Messages.UserNotFound);
                }
                var roomToAdd = _roomDal.GetOne(room => room.RoomName == parsedRoomName);
                if (roomToAdd == null)
                {
                    return new ErrorResult(Messages.RoomMessages.RoomDoesNotExist);
                }

                if (roomToAdd.Users.Contains(userToAdd))
                {
                    return new ErrorResult(Messages.RoomMessages.UserAlreadyExists);
                }
                roomToAdd.Users.Add(userToAdd);
                _roomDal.Update(roomToAdd.Id.ToString(), roomToAdd);

                return new SuccessResult(Messages.RoomMessages.UserAddedToRoom(userToAdd, roomToAdd));
            }

            return new ErrorResult(Messages.GuidError);
        }

        public IResult CreateRoom(string creatorEmail)
        {
            var assignedName = Guid.NewGuid();
            var creator = _userService.GetByMail(creatorEmail).Data;
            _roomDal.Create(new Room
            {
                RoomName = assignedName,
                Creator = creator,
                Users = new List<User>()
            });

            AddUserToRoom(creator.Email, assignedName.ToString());

            return new SuccessResult(Messages.RoomMessages.RoomCreated);
        }

        public IResult DeleteRoom(string creatorEmail, string roomName)
        {
            if (Guid.TryParse(roomName, out var assignedName))
            {
                var roomToDelete = _roomDal.GetOne(room => room.RoomName == assignedName);
                if (creatorEmail == roomToDelete.Creator.Email)
                {
                    _roomDal.Delete(roomToDelete);

                    return new SuccessResult(Messages.RoomMessages.RoomDeleted(roomToDelete));
                }

                return new ErrorResult(Messages.RoomMessages.UserIsNotTheCreator);
            }

            return new ErrorResult(Messages.GuidError);
        }

        public IDataResult<Room> GetRoomByName(string roomName)
        {
            var assignedName = new Guid(roomName);
            return new SuccessDataResult<Room>(_roomDal.GetOne(room => room.RoomName == assignedName));
        }

        public IDataResult<List<Room>> GetRooms()
        {
            return new SuccessDataResult<List<Room>>(_roomDal.GetList());
        }

        public IResult RemoveUserFromRoom(string email, string roomName)
        {
            if (Guid.TryParse(roomName, out var assignedName))
            {
                var userToRemove = _userService.GetByMail(email).Data;

                if(userToRemove == null)
                {
                    return new ErrorResult(Messages.UserNotFound);
                }

                var removeRoom = _roomDal.GetOne(room => room.RoomName == assignedName);

                if (removeRoom == null)
                {
                    return new ErrorResult(Messages.RoomMessages.RoomDoesNotExist);
                }

                if (!removeRoom.Users.Contains(userToRemove))
                {
                    return new ErrorResult(Messages.RoomMessages.UserIsNotInThisRoom);
                }

                if (userToRemove.Email == removeRoom.Creator.Email)
                {
                    return DeleteRoom(userToRemove.Email, removeRoom.RoomName.ToString());
                }

                removeRoom.Users.Remove(userToRemove);
                _roomDal.Update(removeRoom.Id.ToString(), removeRoom);

                return new SuccessResult(Messages.RoomMessages.UserRemovedFromRoom(userToRemove, removeRoom));
            }

            return new ErrorResult(Messages.GuidError);
        }
    }
}
