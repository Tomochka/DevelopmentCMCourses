namespace SocialDBViewer
{
    using System.Collections.Generic;
    using SocialDBViewer.Models;
    using SocialDBViewer.Domain;
    using System.Linq;
    using System;
    using SocialDBViewer.DataAccess.EntityFramework;

    class SocialDataSource
    {
        public List<UserD> _users;
        public List<FriendD> _friends;
        public List<MessageD> _messages;

        private readonly DataContext _dataContext;

        public SocialDataSource(DataContext dataContext)
        {
            _dataContext = dataContext;
            GetUsers();
            GetFriends();
            GetMessages();
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

        public void GetUsers()
        {
            _users = new List<UserD>(_dataContext.Users);
        }

        public void GetFriends()
        {
            _friends = new List<FriendD>(_dataContext.Friends);
        }

        public void GetMessages()
        {
            _messages = new List<MessageD>(_dataContext.Messages);

            foreach (var message in _messages) 
            {
                var likes = new List<LikeD>(_dataContext.Likes).Where(x => x.Message.MessageId == message.MessageId).Select(x => x.User.UserId);
                message.Likes = new List<int>(likes);
            }
            
        }
        
        public UserD GetUserInformation(string userName)
        {
            var userInformation = new UserD();
            var foundUser = _users.Where(user => user.Name == userName);

            try
            {
                userInformation.Name = foundUser.Single().Name;
                userInformation.UserId = foundUser.Single().UserId;
                userInformation.DateOfBirth = foundUser.Single().DateOfBirth;
                userInformation.Gender = foundUser.Single().Gender;
                userInformation.LastVisit = foundUser.Single().LastVisit;
                userInformation.Online = foundUser.Single().Online;
            }
            catch (InvalidOperationException)
            {

                Console.WriteLine("Такого имени пользователя нет в БД или таких имен несколько");
            }
            
            return userInformation;
        }

        public List<UserInformation> GetUserFriends(int userId)
        {
            var foundUserFriends1 = _friends.Where(friend => (friend.FromUser.UserId == userId && friend.Status == 2)
                                  || (friend.ToUser.UserId == userId && friend.Status == 2));

            var foundUserFriends2 = _friends.SelectMany(friend2 => _friends.Where(friend1 => friend1.FromUser.UserId == userId && friend1.Status != 2 && friend1.Status != 3
                                  && friend1.ToUser.UserId == friend2.FromUser.UserId && friend2.Status != 2 && friend2.Status != 3));

            var foundUserFriends = foundUserFriends1.Union(foundUserFriends2);
            var userFriends = new List<UserInformation>();


            foreach (var friend in foundUserFriends)
            {
                var friendId = (friend.ToUser.UserId == userId) ? friend.FromUser.UserId: friend.ToUser.UserId;

                userFriends.Add(UserInf(friendId));
            }

            return userFriends.Distinct().ToList();
        }

        public List<UserInformation> GetUserOnlineFriends(List<UserInformation> userFriends)
        {
            return userFriends.Where(userFriend => userFriend.Online).ToList();
        }

        public List<UserInformation> GetUserSubscribers(int userId)
        {
            var userSubscribers = new List<UserInformation>();
            var foundUserSubscribers1 = _friends.Where(friend => friend.ToUser.UserId == userId && friend.Status != 2 && friend.Status != 3);
            var foundUserSubscribers2 = _friends.SelectMany(friend1 => _friends.Where(friend2 => friend1.FromUser.UserId == userId && friend1.Status != 2 && friend1.Status != 3
                                  && friend1.ToUser.UserId == friend2.FromUser.UserId && friend2.Status != 2 && friend2.Status != 3));

            var foundUserSubscribers = foundUserSubscribers1.Except(foundUserSubscribers2);

            foreach (var subscribers in foundUserSubscribers)
            {
                userSubscribers.Add(UserInf(subscribers.FromUser.UserId));
            }

            return userSubscribers.Distinct().ToList();
        }

        public List<UserInformation> GetUserFriendshipOffers(UserD user)
        {
            var userFriendshipOffers = new List<UserInformation>();
            var foundUserFriendshipOffers = _friends.Where(friend => friend.ToUser.UserId == user.UserId && friend.SendDate > user.LastVisit && friend.Status != 3);

            foreach (var friendshipOffer in foundUserFriendshipOffers)
            {
                userFriendshipOffers.Add(UserInf(friendshipOffer.FromUser.UserId));
            }
            
            return userFriendshipOffers.Distinct().ToList();
        }

        public List<News> GetUserNews(UserD user)
        {
            var userNews = new List<News>();
            var foundUserNews = _messages.Where(message => message.SendDate > user.LastVisit);

            foreach (var currentUserNews in foundUserNews)
            {
                var newsInf = new News
                {
                    AuthorId = currentUserNews.User.UserId,
                    AuthorName = UserInf(currentUserNews.User.UserId).Name,
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
