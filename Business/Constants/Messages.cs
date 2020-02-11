using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Constants
{
    public class Messages
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
