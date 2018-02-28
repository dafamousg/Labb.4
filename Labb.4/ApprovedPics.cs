using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Labb._4
{
    class ApprovedPics
    {
        [BsonId]
        public ObjectId Obj_id { get; set; }

        [BsonElement("Id")]
        public int Id { get; set; }

        [BsonElement("Picture")]
        public string Picture { get; set; }

        public ApprovedPics(int id, string picture)
        {
            Id = id;
            Picture = picture;
        }
    }
}
