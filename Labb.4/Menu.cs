using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Labb._4
{
    class Menu
    {
        public void MainMenu()
        {
            const string connectionString = @"mongodb://dafamousg:lcV26RwzW4o8sc6MmyZKZQHYfvtSrBOTRXYeHVsct2g4TA52XpqXoIdKOfRn9ntt9G35e6VbQqCApMqg52bGZA==@dafamousg.documents.azure.com:10255/?ssl=true&replicaSet=globaldb";

            MongoClient client = new MongoClient(connectionString);

            var db = client.GetDatabase("Labb4");
            var UsersCollection = db.GetCollection<Users>("Users");
            var ReviewingPicsCollection = db.GetCollection<ReviewingPics>("Reviewing_Pictures");
            var ApprovedPicsCollection = db.GetCollection<ApprovedPics>("ApprovedPics");

            string menuChoice;

            do
            {
                Console.WriteLine("Press '1' to add a user");
                Console.WriteLine("Press '2' to show all users");
                Console.WriteLine("Press '3' to show pictures being reviewed");
                Console.WriteLine("Press '4' to show approved pictures");
                Console.WriteLine("Press '5' to quit program");

                menuChoice = Console.ReadLine();

                switch (menuChoice)
                {
                    case "1":
                        AddUser(UsersCollection, ReviewingPicsCollection);
                        break;

                    case "2":
                        ShowAllUsers(UsersCollection);
                        break;

                    case "3":
                        ShowReviewingPics(ReviewingPicsCollection);
                        break;

                    case "4":
                        ShowApprovedPics(ApprovedPicsCollection);
                        break;

                    case "5":
                        Console.Clear();
                        Continue();
                        break;

                    default:
                        Console.Clear();
                        Console.WriteLine("Wrong input, please choose a valid option..");
                        Continue();
                        break;
                }

            } while (menuChoice != "5");

        }

        //Adds users and pic "Flie" to DB
        public static void AddUser(IMongoCollection<Users> userCollection, IMongoCollection<ReviewingPics> collection)
        {
            Console.Clear();
            int maxUsers = 0;
            
            var Users = collection.Find(new BsonDocument()).ToList();
            if (Users.Count > 0)
            {

                maxUsers = Users.Last().Id + 1;

            }
            else
            {
                maxUsers = 0;
            }
            
            Console.Clear();

            Console.WriteLine("Enter your email. (Ex: Per.Nicklas@hotmail.com)");
            string email = Console.ReadLine();

            Console.Clear();
            Console.WriteLine("Please enter the full name of your picture? (Including the extensions. Ex: .png)");
            string picture = Console.ReadLine();


            userCollection.InsertOne(new Users(maxUsers, email));
            collection.InsertOne(new ReviewingPics(maxUsers, picture));

            Continue();
        }

        //checks and shows all users in DB
        public static void ShowAllUsers(IMongoCollection<Users> collection)
        {
            Console.Clear();

            var list = collection.Find(new BsonDocument()).ToList();

            if (list.Count > 0)
            {
                foreach (var users in list)
                {
                    Console.WriteLine($"ID: {users.Id}, Email: {users.Email}");
                }
            }
            else
            {
                Console.WriteLine("There are no users in list");

            }

            Continue();
        }

        //checks and shows all Reviewing pics in DB
        public static void ShowReviewingPics(IMongoCollection<ReviewingPics> collection)
        {
            Console.Clear();

            var list = collection.Find(new BsonDocument()).ToList();

            if(list.Count > 0)
            {
                foreach (var item in list)
                {
                    Console.WriteLine($"ID: {item.Id}, Pic: {item.Picture}");
                }
            }
            else
            {
                Console.WriteLine("There are no pics in que.");
            }

            Continue();
        }

        //checks and shows all approved pics in DB
        public static void ShowApprovedPics(IMongoCollection<ApprovedPics> collection)
        {
            Console.Clear();

            var list = collection.Find(new BsonDocument()).ToList();

            if (list.Count > 0)
            {
                foreach (var item in list)
                {
                    Console.WriteLine($"ID: {item.Id}, Pic: {item.Picture}");
                }
            }
            else
            {
                Console.WriteLine("There are no approved pics.");
            }
            
            Continue();
        }

        public static void Continue()
        {
            Console.WriteLine("\nPress enter to continue..");
            Console.ReadKey();
            Console.Clear();
        }

    }
}
