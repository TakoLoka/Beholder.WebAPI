using Core.Abstract;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Entities.Models
{
    public class Room: AbstractMongoEntity
    {
        [BsonElement("RoomName")]
        public Guid RoomName { get; set; }
        [BsonElement("Users")]
        public List<User> Users { get; set; }
    }
}
