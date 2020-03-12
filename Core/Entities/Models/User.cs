using Core.Abstract;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Entities.Models
{
    public class User : AbstractMongoEntity
    {
        [BsonElement("Name")]
        public string FirstName { get; set; }
        [BsonElement("LastName")]
        public string LastName { get; set; }
        [BsonElement("Email")]
        public string Email { get; set; }
        [BsonElement("PasswordHash")]
        public byte[] PasswordHash { get; set; }
        [BsonElement("PasswordSalt")]
        public byte[] PasswordSalt { get; set; }
        [BsonElement("RegistryDate")]
        public DateTime RegistryDate { get; set; }
        [BsonElement("BirthDay")]
        public DateTime BirthDay { get; set; }
        [BsonElement("OperationClaims")]
        public List<OperationClaim> OperationClaims { get; set; }
        [BsonElement("Characters")]
        public List<Character> Characters { get; set; }
    }
}
