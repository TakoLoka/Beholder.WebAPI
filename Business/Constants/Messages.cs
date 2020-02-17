using Core.Entities.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Constants
{
    public class Messages
    {
        #region Global Messages
        public static string GuidError = "The Room Name is not valid.";
        public static string UserNotFound = "User Not Found";
        public static string UserFound = "User Found";
        #endregion

        public class RoomMessages
        {
            public static string UserAddedToRoom(User user, Room room) => user.Email + ": Added to Room: " + room.RoomName;
            public static string RoomCreated = "Room Created";
            public static string UserIsNotTheCreator = "The user is not the creator of this room.";
            public static string RoomDoesNotExist = "The Room does not exist";

            public static string UserIsNotInThisRoom = "User is NOT a part of this room";
            public static string UserAlreadyExists = "User is already a part of this room";

            public static string RoomDeleted(Room room) => "Room " + room.RoomName + ": Deleted";
            public static string UserRemovedFromRoom(User user, Room room) => user.Email + ": Removed from Room: " + room.RoomName;
        }

        public class UserMessages
        {

            public static string UserCreated(User user) => user.Email + ": Created";
            public static string UserBecamePremiumDM(User user) => user.Email + ": Became Premium Dungeon Master";
            public static string UserBecamePremiumPlayer(User user) => user.Email + ": Became Premium Player";
            public static string UserAlreadyPremiumDM(User user) => user.Email + ": Already Premium Dungeon Master";
            public static string UserAlreadyPremiumPlayer(User user) => user.Email + ": Already Premium Player";
        }

        public class AuthMessages
        {
            #region Authentication and Authorization Messages
            public static string UserAlreadyExists = "User Already Exists";
            public static string PasswordError = "Incorrect Password";
            public static string LoginSuccessful = "Successful Login";
            public static string RegisterSuccessful = "User Successfully Registered";
            public static string AccessTokenCreated = "Access Token Created";
            #endregion
        }
    }
}
