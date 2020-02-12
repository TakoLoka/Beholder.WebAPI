﻿using Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Constants
{
    public class Messages
    {
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
            public static string UserNotFound = "User Not Found";
            public static string UserFound = "User Found";
            public static string UserAlreadyExists = "User Already Exists";
            public static string PasswordError = "Incorrect Password";
            public static string LoginSuccessful = "Successful Login";
            public static string RegisterSuccessful = "User Successfully Registered";
            public static string AccessTokenCreated = "Access Token Created";
            #endregion
        }
    }
}
