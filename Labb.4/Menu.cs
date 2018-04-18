
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualBasic;

namespace Labb._4
{
    public class Menu
    {
        private static string UserEmailInput { get; set; }
        private static string PictureNameInput { get; set; }
        private static string FormatType { get; set; }

        protected static bool EmailIsCorrect { get; set; }
        protected static bool PictureFormatIsCorrect { get; set; }

        public void MainMenu()
        {
            const string connectionString = @"mongodb://labb4mongodb:thvGuqhUCMdhSuhsup6VXFyLrHrpvYhMlcfzeNee8xo2VmDl9A2fvyvR8oPti0qviWiIXTwHuTSDDPH800tigA==@labb4mongodb.documents.azure.com:10255/?ssl=true&replicaSet=globaldb";

            MongoClient client = new MongoClient(connectionString);

            var db = client.GetDatabase("Labb4DB");
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

        //Adds users and pic "File" to DB
        static void AddUser(IMongoCollection<Users> userCollection, IMongoCollection<ReviewingPics> imageCollection)
        {
            EmailIsCorrect = false;
            PictureFormatIsCorrect = false;

            Console.Clear();
            int maxUsers = 0;

            var Users = userCollection.Find(new BsonDocument()).ToList();
            if (Users.Count > 0)
            {
                maxUsers = Users.Last().Id + 1;
            }
            else
            {
                maxUsers = 0;
            }

            Console.WriteLine("Enter your email. (Ex: Per.Nicklas@hotmail.com)");

            while (!EmailIsCorrect)
            {
                UserEmailInput = Console.ReadLine();

                if (IsEmailValid(UserEmailInput))
                {
                    EmailIsCorrect = true;
                }
                else
                {
                    Console.WriteLine("You've entered an incorrect email. Please try again:");
                }

            }

            Console.WriteLine("Enter the image name with the following formats:\n.jpg, .png, .tif or .bmp (Ex: derpydog.jpg)");

            while (!PictureFormatIsCorrect)
            {
                PictureNameInput = Console.ReadLine();
                FormatType = PictureNameInput.Substring(PictureNameInput.Length - Math.Min(4, PictureNameInput.Length));

                if (FormatType == ".jpg" || FormatType == ".png" || FormatType == ".tif" || FormatType == ".bmp")
                {
                    PictureFormatIsCorrect = true;
                }
                else
                {
                    Console.WriteLine("You've entered a wrong picture format, please try again:");
                }
            }

            userCollection.InsertOne(new Users(maxUsers, UserEmailInput));
            imageCollection.InsertOne(new ReviewingPics(maxUsers, PictureNameInput));

            Continue();
        }

        //checks and shows all users in DB
        static void ShowAllUsers(IMongoCollection<Users> collection)
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
                Console.WriteLine("There are no users in list.");

            }

            Continue();
        }

        //checks and shows all Reviewing pics in DB
        static void ShowReviewingPics(IMongoCollection<ReviewingPics> collection)
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
                Console.WriteLine("There are no pics in que.");
            }

            Continue();
        }

        //checks and shows all approved pics in DB
        static void ShowApprovedPics(IMongoCollection<ApprovedPics> collection)
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

        public static bool IsEmailValid(string emailaddress)
        {
            try
            {
                MailAddress m = new MailAddress(emailaddress);

                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }
    }
}
