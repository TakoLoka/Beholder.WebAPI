using Core.Abstract;
using Core.Entities.Models;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Core.Entities.Models
{
    public class Message : AbstractMongoEntity
    {
        [Required]
        [BsonElement("User")]
        public User User { get; set; }
        [Required]
        [BsonElement("Data")]
        public AbstractMessageModel Data { get; set; }
        [BsonElement("Time")]
        public DateTime Time { get; set; }
    }
}
