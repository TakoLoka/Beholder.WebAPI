using Core.Entities;
using DataAccess.Abstract;
using DataAccess.Database.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAccess.Concrete
{
    public class UserMongoRepository: BaseMongoRepository<User>, IUserDal
    {
        public UserMongoRepository(): base(DB.ConnectionString, DB.DbName, DB.Collections.UserCollection)
        {
        }
    }
}
