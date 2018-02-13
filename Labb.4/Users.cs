using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Labb._4
{
    class Users
    {
        [BsonId]
        public ObjectId _ID { get; set; }

        [BsonElement("ID")]
        public int ID { get; set; }

        [BsonElement("Email")]
        public string Email { get; set; }

        public Users(int id, string email)
        {
            ID = id;
            Email = email;
        }
    }
}
