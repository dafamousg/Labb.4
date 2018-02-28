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
        public ObjectId Obj_id { get; set; }

        [BsonElement("Id")]
        public int Id { get; set; }

        [BsonElement("Email")]
        public string Email { get; set; }

        public Users(int id, string email)
        {
            Id = id;
            Email = email;
        }
    }
}
