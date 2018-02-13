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
        public ObjectId _ID { get; set; }

        [BsonElement("ID")]
        public int ID { get; set; }

        [BsonElement("Picture")]
        public string Picture { get; set; }

        public ApprovedPics(int id, string picture)
        {
            ID = id;
            Picture = picture;
        }
    }
}
