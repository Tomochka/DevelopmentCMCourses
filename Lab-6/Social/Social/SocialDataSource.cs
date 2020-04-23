namespace Social
{
    using System.Collections.Generic;
    using Social.Models;
    using System.Text.Json;
    using System.IO;
    using System.Linq;
    using System;


    public class SocialDataSource
    {
        public List<User> _users;
        public List<Friend> _friends;
        public List<Message> _messages;

        public SocialDataSource(string pathUsers, string pathFriends, string pathMessages)
        {
            GetUsers(pathUsers);
            GetFriends(pathFriends);
            GetMessages(pathMessages);
        }

        public UserContext GetUserContext(string userName)
        {
            var userContext = new UserContext();

            userContext.User = GetUserInformation(userName);
            userContext.Friends = GetUserFriends(userContext.User.UserId);
            userContext.OnlineFriends = GetUserOnlineFriends(userContext.Friends);
            userContext.Subscribers = GetUserSubscribers(userContext.User.UserId);
            userContext.FriendshipOffers = GetUserFriendshipOffers(userContext.User);
            userContext.News = GetUserNews(userContext.User);

            return userContext;
        }

        public  void GetUsers(string path)
        {
            var jsonString = File.ReadAllText(path);
            User[] users = JsonSerializer.Deserialize<User[]>(jsonString);

            _users = new List<User>(users);

        }

        public void GetFriends(string path)
        {
            var jsonString = File.ReadAllText(path);
            Friend[] friends = JsonSerializer.Deserialize<Friend[]>(jsonString);

            _friends = new List<Friend>(friends);
        }

        public void GetMessages(string path)
        {
            var jsonString = File.ReadAllText(path);
            Message[] messages = JsonSerializer.Deserialize<Message[]>(jsonString);

            _messages = new List<Message>(messages);
        }
        
        public User GetUserInformation(string userName)
        {
            /* var foundUser2 = from user in _users
                            where user.Name == userName
                            select user;*/

            //var userInformation = new User();
            var users = _users.Where(user => user.Name == userName);
            if (users.Count() > 1)
            {
                throw new Exception("Такого имени пользователя нет в БД или таких имен несколько");
            }

            return users.Single();

            //try
            //{
            //    userInformation.Name = foundUser.Single().Name;
            //    userInformation.UserId = foundUser.Single().UserId;
            //    userInformation.DateOfBirth = foundUser.Single().DateOfBirth;
            //    userInformation.Gender = foundUser.Single().Gender;
            //    userInformation.LastVisit = foundUser.Single().LastVisit;
            //    userInformation.Online = foundUser.Single().Online;
            //}
            //catch (InvalidOperationException)
            //{

            //    Console.WriteLine("Такого имени пользователя нет в БД или таких имен несколько");
            //}

            //return userInformation;
        }

        public List<UserInformation> GetUserFriends(int userId)
        {
            /*var foundUserFriends11 = from friend in _friends
                                    where (friend.FromUserId == userId && friend.Status == 2)
                                    || (friend.ToUserId == userId && friend.Status == 2)
                                    select friend;*/

            /*var foundUserFriends22 = from friend1 in _friends
                                    from friend2 in _friends
                                    where friend1.FromUserId == userId && friend1.Status != 2 && friend1.Status != 3
                                    && friend1.ToUserId == friend2.FromUserId && friend2.Status != 2 && friend2.Status != 3
                                    select friend1;*/

            var foundUserFriends1 = _friends.Where(friend => (friend.FromUserId == userId && friend.Status == 2)
                                  || (friend.ToUserId == userId && friend.Status == 2));

            var foundUserFriends2 = _friends.SelectMany(friend2 => _friends.Where(friend1 => friend1.FromUserId == userId && friend1.Status != 2 && friend1.Status != 3
                                  && friend1.ToUserId == friend2.FromUserId && friend2.Status != 2 && friend2.Status != 3));

            var foundUserFriends = foundUserFriends1.Union(foundUserFriends2);
            var userFriends = new List<UserInformation>();


            foreach (var friend in foundUserFriends)
            {
                var friendId = (friend.ToUserId == userId) ? friend.FromUserId : friend.ToUserId;

                userFriends.Add(UserInf(friendId));
            }

            return userFriends.Distinct().ToList();
        }

        public  List<UserInformation> GetUserOnlineFriends(List<UserInformation> userFriends)
         {
            return userFriends.Where(userFriend => userFriend.Online).ToList();
         }

        public List<UserInformation> GetUserSubscribers(int userId)
        {
            var userSubscribers = new List<UserInformation>();
            var foundUserSubscribers1 = _friends.Where(friend => friend.ToUserId == userId && friend.Status != 2 && friend.Status != 3);
            var foundUserSubscribers2 = _friends.SelectMany(friend1 => _friends.Where(friend2 => friend1.FromUserId == userId && friend1.Status != 2 && friend1.Status != 3
                                  && friend1.ToUserId == friend2.FromUserId && friend2.Status != 2 && friend2.Status != 3));

            var foundUserSubscribers = foundUserSubscribers1.Except(foundUserSubscribers2);

            foreach (var subscribers in foundUserSubscribers)
            {
                userSubscribers.Add(UserInf(subscribers.FromUserId));
            }

            return userSubscribers.Distinct().ToList();
        }

        public List<UserInformation> GetUserFriendshipOffers(User user)
        {
            var userFriendshipOffers = new List<UserInformation>();
            var foundUserFriendshipOffers = _friends.Where(friend => friend.ToUserId == user.UserId && friend.SendDate > user.LastVisit && friend.Status != 3);
           
            foreach (var friendshipOffer in foundUserFriendshipOffers)
            {
                userFriendshipOffers.Add(UserInf(friendshipOffer.FromUserId));
            }

            return userFriendshipOffers.Distinct().ToList();
        }

        public List<News> GetUserNews(User user)
        {
            var userNews = new List<News>();
            var foundUserNews = _messages.Where(message => message.SendDate > user.LastVisit);

            foreach (var currentUserNews in foundUserNews) 
            {
                var newsInf = new News
                {
                    AuthorId = currentUserNews.AuthorId,
                    AuthorName = UserInf(currentUserNews.AuthorId).Name,
                    Likes = currentUserNews.Likes,
                    Text = currentUserNews.Text
                };

                userNews.Add(newsInf);
            }

            return userNews;
        }

        private UserInformation UserInf(int userId)
        {
            var userInf = new UserInformation();
            var foundUser = _users.Where(u => u.UserId == userId);

            try
            {
                userInf.UserId = foundUser.Single().UserId;
                userInf.Name = foundUser.Single().Name;
                userInf.Online = foundUser.Single().Online;
            }
            catch (InvalidOperationException)
            {
                Console.WriteLine("Такого имени друга пользователя нет в БД или таких имен несколько");
            }

            return userInf;
        }
    }
}
