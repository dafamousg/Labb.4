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

            const string connectionString = @"mongodb://labb4mongodb:thvGuqhUCMdhSuhsup6VXFyLrHrpvYhMlcfzeNee8xo2VmDl9A2fvyvR8oPti0qviWiIXTwHuTSDDPH800tigA==@labb4mongodb.documents.azure.com:10255/?ssl=true&replicaSet=globaldb";


            MongoClient client = new MongoClient(connectionString);

            var db = client.GetDatabase("Labb4DB");
            var UsersCollection = db.GetCollection<Users>("Users");
            var ReviewingPicsCollection = db.GetCollection<ReviewingPics>("Reviewing_Pictures");
            var ApprovedPicsCollection = db.GetCollection<ApprovedPics>("ApprovedPics");

            string validID = "The entered ID does not exist, please enter valid ID.";
            string urlCommands = "Please type a command viewReviewQueue or approve/reject a picture by ID.";

            // parse query parameter
            var mode = req.GetQueryNameValuePairs()
                .FirstOrDefault(q => string.Compare(q.Key, "mode", true) == 0)
                .Value;

            var id = req.GetQueryNameValuePairs()
                .FirstOrDefault(q => string.Compare(q.Key, "id", true) == 0)
                .Value;


            dynamic data = await req.Content.ReadAsAsync<object>();

            var result = ReviewingPicsCollection.Find(new BsonDocument()).ToList();

            List<string> listOfObj = new List<string>();


            if (result.Count > 0)
            {
                foreach (var x in result)
                {
                    listOfObj.Add($"ID: {x.Id}, Picture: {x.Picture}");
                }
            }
            else
            {

            }

            if (mode == null)
                mode = "";

            switch (mode.ToUpper())
            {
                case "VIEWREVIEWQUEUE":
                    if (listOfObj.Count > 0)
                        return req.CreateResponse(HttpStatusCode.OK, listOfObj);
                    else
                        return req.CreateResponse(HttpStatusCode.OK, "There are no pictures that need to be reviewed");
                case "REJECT":
                    if (id != null)
                    {
                        var findID = ReviewingPicsCollection.Find(q => q.Id == int.Parse(id));

                        if (findID.Count() == 0)
                            return req.CreateResponse(HttpStatusCode.OK, validID);
                        else
                        {
                            var filter = Builders<ReviewingPics>.Filter.Eq("Id", id);
                            ReviewingPicsCollection.DeleteOne(filter);
                            return req.CreateResponse(HttpStatusCode.OK, "The picture was successfully removed.");
                        }
                    }
                    else
                        return req.CreateResponse(HttpStatusCode.OK, "Please enter an ID.");
                case "APPROVE":
                    if (id != null)
                    {
                        var findID = ReviewingPicsCollection.Find(q => q.Id == int.Parse(id));

                        if (findID.Count() == 0)
                            return req.CreateResponse(HttpStatusCode.OK, validID);
                        else
                        {
                            int tempID = findID.First().Id;
                            string tempPic = findID.First().Picture;
                            ApprovedPicsCollection.InsertOne(new ApprovedPics(tempID, tempPic));
                            var filter = Builders<ReviewingPics>.Filter.Eq("Id", id);
                            ReviewingPicsCollection.DeleteOne(filter);
                            return req.CreateResponse(HttpStatusCode.OK, "The picture was approved.");
                        }
                    }
                    else
                        return req.CreateResponse(HttpStatusCode.OK, "Failed");

                default:
                    return req.CreateResponse(HttpStatusCode.BadRequest, urlCommands);
            }
        }
    }
}
