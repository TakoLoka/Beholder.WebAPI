using Core.Abstract;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Entities.Models
{
    public class Room: AbstractMongoEntity
    {
        [BsonElement("RoomId")]
        public Guid RoomId { get; set; }
        [BsonElement("RoomName")]
        public string RoomName { get; set; }
        [BsonElement("Description")]
        public string Description { get; set; }
        [BsonElement("Creator")]
        public User Creator { get; set; }
        [BsonElement("Users")]
        public List<User> Users { get; set; }
    }
}
