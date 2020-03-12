using Core.Abstract;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Core.Entities.Models
{
    public class OperationClaim: AbstractMongoEntity
    {
        [BsonElement("Name")]
        public string Name { get; set; }
    }
}
