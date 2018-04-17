using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using System.Collections.Generic;

namespace HttpFunctions
{
    public static class ApproveRejectPictures
    {
        [FunctionName("ApproveRejectPictures")]
        public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)]HttpRequestMessage req, TraceWriter log)
        {
            log.Info($"C# HTTP trigger function processed a request.");

            const string connectionString = @"mongodb://dafamousg:lcV26RwzW4o8sc6MmyZKZQHYfvtSrBOTRXYeHVsct2g4TA52XpqXoIdKOfRn9ntt9G35e6VbQqCApMqg52bGZA==@dafamousg.documents.azure.com:10255/?ssl=true&replicaSet=globaldb";


            MongoClient client = new MongoClient(connectionString);

            var db = client.GetDatabase("Labb4DB");
            var UsersCollection = db.GetCollection<Users>("Users");
            var ReviewingPicsCollection = db.GetCollection<ReviewingPics>("Reviewing_Pictures");
            var ApprovedPicsCollection = db.GetCollection<ApprovedPics>("ApprovedPics");


            // parse query parameter
            var mode = req.GetQueryNameValuePairs()
                .FirstOrDefault(q => string.Compare(q.Key, "mode", true) == 0) 
                .Value;

            var id = req.GetQueryNameValuePairs()
                .FirstOrDefault(q => string.Compare(q.Key, "id", true) == 0)
                .Value;

            //Switchs som bestämmer vad som ska hända
            switch (mode.ToUpper())
            {
                case "VIEWREVIEWQUEUE":
                    var reviewsBson = GetReviewQueue(db);
                    List<string> pictures = new List<string>();
                    foreach (var item in reviewsBson)
                    {
                        //string imgPath = item.Elements.FirstOrDefault(e => string.Compare(e.Name, "path", true) == 0).Value.ToString();
                        pictures.Add(item.Values.ToJson());
                    }
                    return req.CreateResponse(HttpStatusCode.OK, pictures);
                case "APPROVE":
                    var pictureForApproveQueue = db.GetCollection<BsonDocument>("pictures");
                    var filter = Builders<BsonDocument>.Filter.Eq("_id", ObjectId.Parse(id));
                    var findSpecPicture = pictureForApproveQueue.Find(filter);
                    if (findSpecPicture.Count() != 0)
                    {
                        var update = Builders<BsonDocument>.Update.Set("approved", true);
                        pictureForApproveQueue.UpdateOne(filter, update);
                        return req.CreateResponse(HttpStatusCode.OK, "Successfully approved picture with id " + id);
                    }
                    else
                    {
                        return req.CreateResponse(HttpStatusCode.BadRequest, "The given id does not match any id in the database");
                    }
                case "REJECT":
                    var pictureForRejectQueue = db.GetCollection<BsonDocument>("pictures");
                    filter = Builders<BsonDocument>.Filter.Eq("_id", ObjectId.Parse(id));
                    var findSpecPictureForReject = pictureForRejectQueue.Find(filter);
                    if (findSpecPictureForReject.Count() != 0)
                    {
                        pictureForRejectQueue.DeleteOne(filter);
                        return req.CreateResponse(HttpStatusCode.OK, "Successfully rejected and removed picture with id " + id);
                    }
                    else
                    {
                        return req.CreateResponse(HttpStatusCode.BadRequest, "The given id does not match any id in the database");
                    }
                default:
                    return req.CreateResponse(HttpStatusCode.BadRequest, "Invalid mode");
            }



            //// Get request body
            //dynamic data = await req.Content.ReadAsAsync<object>();

            //// Set name to query string or body data
            //name = name ?? data?.name;

            //return name == null
            //   ? req.CreateResponse(HttpStatusCode.BadRequest, "Please pass a name on the query string or in the request body")
            //    : req.CreateResponse(HttpStatusCode.OK, "Hello " + name);
        }

        public static List<BsonDocument> GetReviewQueue(IMongoDatabase db)
        {
            var pictureCollection = db.GetCollection<BsonDocument>("pictures");

            var match = new BsonDocument
            {
                {
                    "$match",
                    new BsonDocument
                    {
                        {"approved", false }
                    }
                }
            };

            var pipeline = new[] { match };

            return pictureCollection.Aggregate<BsonDocument>(pipeline).ToList();
        }
    }
}
