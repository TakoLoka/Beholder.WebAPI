using Business.Abstract;
using Business.Constants;
using Business.ValidationRules.FluentValidation.RoomValidation;
using Core.Aspects.Autofac.Validation;
using Core.Dtos.RoomDtos;
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
        [ValidationAspect(typeof(CreateRoomValidation), Priority = 1)]
        public IResult CreateRoom(CreateRoomDto createRoomDto)
        {
            var assignedId = Guid.NewGuid();
            var userWithMail = _userService.GetByMail(createRoomDto.UserEmail);

            if (!userWithMail.Success)
            {
                return new ErrorResult(Messages.UserNotFound);
            }

            _roomDal.Create(new Room
            {
                RoomId = assignedId,
                Creator = userWithMail.Data,
                RoomName = createRoomDto.RoomName,
                Description = createRoomDto.Description
            });

            return new SuccessResult(assignedId.ToString());
        }

        public IDataResult<Room> GetRoomById(string roomId)
        {
            var assignedId = new Guid(roomId);
            return new SuccessDataResult<Room>(_roomDal.GetOne(room => room.RoomId.Equals(assignedId)));
        }

        public IDataResult<List<Room>> GetRooms()
        {
            return new SuccessDataResult<List<Room>>(_roomDal.GetList());
        }

        public IDataResult<Room> GetRoomByName(string roomName)
        {
            return new SuccessDataResult<Room>(_roomDal.GetOne(room => room.RoomName.Equals(roomName)));
        }
        
        public IDataResult<List<Room>> GetRoomsWithUser(string userEmail)
        {
            var usr = _userService.GetByMail(userEmail).Data;
            return new SuccessDataResult<List<Room>>(_roomDal.GetList(room => room.Users.Contains(usr)));
        }
        [ValidationAspect(typeof(DeleteRoomValidation), Priority = 1)]
        public IResult DeleteRoom(DeleteRoomDto deleteRoomDto)
        {
            if (Guid.TryParse(deleteRoomDto.RoomId.ToString(), out var assignedId))
            {
                var roomToDelete = _roomDal.GetOne(rm => rm.RoomId.Equals(assignedId));
                if (deleteRoomDto.UserEmail.Equals(roomToDelete.Creator.Email))
                {
                    _roomDal.Delete(roomToDelete);

                    return new SuccessResult(Messages.RoomMessages.RoomDeleted(roomToDelete));
                }

                return new ErrorResult(Messages.RoomMessages.UserIsNotTheCreator);
            }

            return new ErrorResult(Messages.GuidError);
        }
        [ValidationAspect(typeof(AddUserToRoomValidation), Priority = 1)]
        public IResult AddUserToRoom(AddUserToRoomDto userJoinRoomDto)
        {
            if (Guid.TryParse(userJoinRoomDto.RoomId.ToString(), out var parsedRoomId))
            {
                var userToAdd = _userService.GetByMail(userJoinRoomDto.UserEmail).Data;
                if (userToAdd == null)
                {
                    return new ErrorResult(Messages.UserNotFound);
                }
                var roomToAdd = _roomDal.GetOne(rm => rm.RoomId.Equals(parsedRoomId));
                if (roomToAdd == null)
                {
                    return new ErrorResult(Messages.RoomMessages.RoomDoesNotExist);
                }

                if (roomToAdd.Users == null)
                    roomToAdd.Users = new List<User>();

                if (roomToAdd.Users.Find(user => user.Email.Equals(userToAdd.Email)) != null)
                {
                    return new ErrorResult(Messages.RoomMessages.UserAlreadyExists);
                }

                roomToAdd.Users.Add(userToAdd);
                _roomDal.Update(roomToAdd.Id.ToString(), roomToAdd);

                return new SuccessResult(Messages.RoomMessages.UserAddedToRoom(userToAdd, roomToAdd));
            }

            return new ErrorResult(Messages.GuidError);
        }
        [ValidationAspect(typeof(RemoveUserFromRoomValidation), Priority = 1)]
        public IResult RemoveUserFromRoom(RemoveUserFromRoomDto removeUserFromRoomDto)
        {
            if (Guid.TryParse(removeUserFromRoomDto.RoomId.ToString(), out var assignedId))
            {

                var removeRoom = _roomDal.GetOne(rm => rm.RoomId.Equals(assignedId));

                if (removeRoom == null)
                {
                    return new ErrorResult(Messages.RoomMessages.RoomDoesNotExist);
                }

                var userToRemove = removeRoom.Users.Find(x => x.Email.Equals(removeUserFromRoomDto.UserEmail));

                if (removeRoom.Users.Find(usr => usr.Email.Equals(userToRemove.Email)) == null)
                {
                    return new ErrorResult(Messages.RoomMessages.UserIsNotInThisRoom);
                }

                removeRoom.Users.Remove(userToRemove);
                _roomDal.Update(removeRoom.Id.ToString(), removeRoom);

                return new SuccessResult(Messages.RoomMessages.UserRemovedFromRoom(userToRemove, removeRoom));
            }

            return new ErrorResult(Messages.GuidError);
        }
        public IResult RemoveUserFromAllRooms(string userEmail)
        {
            var usr = _userService.GetByMail(userEmail).Data;
            List<Room> rooms = _roomDal.GetList(room => room.Users.Contains(usr));
            if (rooms != null && rooms.Count >= 0)
            {
                foreach (var room in rooms)
                {
                    RemoveUserFromRoom(new RemoveUserFromRoomDto { UserEmail = userEmail, RoomId = room.RoomId.ToString() });
                }
            }

            return new SuccessResult(Messages.RoomMessages.UserRemovedFromAllRooms);
        }
    }
}
