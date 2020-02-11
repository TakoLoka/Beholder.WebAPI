using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Abstract
{
    public abstract class AbstractMongoEntity
    {
        public ObjectId Id { get; set; }
    }
}
