namespace SocialDBViewer
{
    using System;
    using System.Configuration;
    using SocialDBViewer.DataAccess.EntityFramework;

    class Program
    {
        static void Main(string[] args)
        {

            if (args.Length == 0)
            {
                Console.WriteLine("User name not specified!");
                return;
            }

            var name = args[0];

            if (args.Length == 2)
            {
                name += " " + args[1];
            }

            if (string.IsNullOrEmpty(name))
            {
                Console.WriteLine("More than two argument was entered");
                return;
            }

            string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnectionString"].ConnectionString;
            var socialDataSource = new SocialDataSource(new DataContext(connectionString));
            var userContext = socialDataSource.GetUserContext(name);

            //User
            DateTime now = DateTime.Today;
            int age = now.Year - userContext.User.DateOfBirth.Year;
            if (userContext.User.DateOfBirth > now.AddYears(-age)) age--;

            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.WriteLine(@"Target user : {0}, {1} years old", userContext.User.Name, age);
            Console.ResetColor();
            Console.WriteLine();

            //Friends
            Console.BackgroundColor = ConsoleColor.DarkGreen;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.WriteLine(@"Number of friends: {0}", userContext.Friends.Count);
            var i = 0;

            foreach (var friend in userContext.Friends)
            {
                i++;
                Console.WriteLine(@"{0}) {1}", i, friend.Name);
            }

            Console.ResetColor();
            Console.WriteLine();

            //Online friends
            Console.BackgroundColor = ConsoleColor.Green;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.WriteLine(@"Number of online friends: {0}", userContext.OnlineFriends.Count);
            i = 0;

            foreach (var onlineFriend in userContext.OnlineFriends)
            {
                i++;
                Console.WriteLine(@"{0}) {1}", i, onlineFriend.Name);
            }

            Console.ResetColor();
            Console.WriteLine();

            //Subscribers
            Console.BackgroundColor = ConsoleColor.DarkYellow;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.WriteLine(@"Number of subscribers: {0}", userContext.Subscribers.Count);
            i = 0;

            foreach (var subscriber in userContext.Subscribers)
            {
                i++;
                Console.WriteLine(@"{0}) {1}", i, subscriber.Name);
            }

            Console.ResetColor();
            Console.WriteLine();

            //Friendship Offers
            Console.BackgroundColor = ConsoleColor.Yellow;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.WriteLine(@"Number of friendship offers: {0}", userContext.FriendshipOffers.Count);
            i = 0;

            foreach (var friendshipOffer in userContext.FriendshipOffers)
            {
                i++;
                Console.WriteLine(@"{0}) {1}", i, friendshipOffer.Name);
            }

            Console.ResetColor();
            Console.WriteLine();

            //News
            Console.BackgroundColor = ConsoleColor.Blue;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.WriteLine("News:");
            Console.WriteLine();
            Console.ResetColor();
            i = 0;

            foreach (var _new in userContext.News)
            {
                i++;
                Console.WriteLine(@"{0}) {1}", i, _new.AuthorName);
                Console.WriteLine(@"Message: {0}", _new.Text);
                Console.WriteLine(@"Likes {0}", _new.Likes.Count);
                Console.WriteLine();
            }

            Console.WriteLine();
            Console.ReadLine();
        }
    }
}
