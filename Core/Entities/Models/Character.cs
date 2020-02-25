using Core.Abstract;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Entities.Models
{
    public class Character:AbstractMongoEntity
    {
        [BsonElement("Name")]
        public string Name { get; set; }
        [BsonElement("Coordinates")]
        public Point Coordinates { get; set; }

        public struct Point
        {
            private int x;
            private int y;

            public Point(int x, int y)
            {
                this.x = x;
                this.y = y;
            }

            public void SetCoordinates(int x, int y)
            {
                this.x = x;
                this.y = y;
            }

            public void GetCoordinates(out int x, out int y)
            {
                x = this.x;
                y = this.y;
            }
        }
    }
}
