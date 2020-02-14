using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DataAccess.Database.Constants
{
    public static class DB
    {
        public static string ConnectionString = "mongodb+srv://TakoLoka:xxxtarem23h.@devconnector-tglkz.mongodb.net/test?retryWrites=true&w=majority";
        public static string DbName = "Beholder";
        public static class Collections
        {
            public static string UserCollection = "users";
            public static string OperationClaimCollection = "operationClaims";
            public static string RoomCollection = "rooms";
            public static string MessageCollection = "messages";
        }
    }
}
