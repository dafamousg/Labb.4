using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Labb._4
{
    class Menu
    {
        public void MainMenu()
        {
            const string connectionString = @"Data Source=dafamousg.database.windows.net;Initial Catalog=Labb4;Integrated Security=False;User ID=ginokalyun;Password=********;Connect Timeout=60;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

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

                        break;

                    case "2":

                        break;

                    case "3":

                        break;

                    case "4":

                        break;

                    case "5":
                        Console.Clear();
                        Console.WriteLine("\nPress enter to exit");
                        Console.ReadKey();
                        break;

                    default:
                        Console.Clear();
                        Console.WriteLine("Wrong input, please choose a valid option..");
                        Console.WriteLine("\nPress enter to continue..");
                        Console.ReadKey();
                        break;
                }

            } while (menuChoice != "5");

        }
        public static void AddUser(IMongoCollection<Users> userCollection, IMongoCollection<ReviewingPics> collection)
        {

        }
        public static void ShowAllUsers(IMongoCollection<Users> collection)
        {

        }
        public static void ShowPicturesInQue(IMongoCollection<ReviewingPics> collection)
        {

        }
        public static void ShowApprovedPics(IMongoCollection<ApprovedPics> collection)
        {

        }

    }
}
