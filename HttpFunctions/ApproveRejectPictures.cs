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

namespace HttpFunctions
{
    public static class ApproveRejectPictures
    {
        [FunctionName("ApproveRejectPictures")]
        public static void Run([TimerTrigger("*/20 * * * * ")]TimerInfo myTimer, TraceWriter log)
        {
            log.Info($"C# Timer trigger executed at: {DateTime.Now}");

            const string connectionString = @"mongodb://dafamousg:lcV26RwzW4o8sc6MmyZKZQHYfvtSrBOTRXYeHVsct2g4TA52XpqXoIdKOfRn9ntt9G35e6VbQqCApMqg52bGZA==@dafamousg.documents.azure.com:10255/?ssl=true&replicaSet=globaldb";


            MongoClient client = new MongoClient(connectionString);

            var db = client.GetDatabase("Labb4");
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



            // Get request body
            //dynamic data = await req.Content.ReadAsAsync<object>();

            //// Set name to query string or body data
            //name = name ?? data?.name;

            //return name == null
            //    ? req.CreateResponse(HttpStatusCode.BadRequest, "Please pass a name on the query string or in the request body")
            //    : req.CreateResponse(HttpStatusCode.OK, "Hello " + name);
        }
    }
}
