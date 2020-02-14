using Core.Entities.Models;
using DataAccess.Abstract;
using DataAccess.Database.Constants;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Concrete
{
    public class RoomMongoRepository: BaseMongoRepository<Room>, IRoomDal
    {
        public RoomMongoRepository(): base(DB.ConnectionString, DB.DbName, DB.Collections.RoomCollection)
        {

        }
    }
}
