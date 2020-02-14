using Core.Entities.Models;
using DataAccess.Abstract;
using DataAccess.Database.Constants;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Concrete
{
    public class MessageMongoRepository: BaseMongoRepository<Message>, IMessageDal
    {
        public MessageMongoRepository(): base(DB.ConnectionString, DB.DbName, DB.Collections.MessageCollection)
        {
        }
    }
}
