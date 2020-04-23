namespace Social.Tests
{
    using NUnit.Framework;
    using System;

    [TestFixture]
    public class SocialDataSourceTest
    {
        private const string PathDirectory = @"../../../Stubs/";

        [Test]
        public void SocialDataSource_GetInformation_ShouldBeCorrectInformation()
        {
            var PathUsers = PathDirectory + @"GetInformation/users.json";
            var PathFriends = PathDirectory + @"GetInformation/friends.json";
            var PathMessages = PathDirectory + @"GetInformation/messages.json";

            var socialDataSource = new SocialDataSource(PathUsers, PathFriends, PathMessages);
            var user = socialDataSource.GetUserInformation("Elizabeth");

            var check = false;
            var date1 = new DateTime(2019, 2, 3);
            var date2 = new DateTime(1992, 3, 23);

            if (user.Name == "Elizabeth" && user.Gender == 1 && !user.Online && user.UserId == 4
                && user.LastVisit == date1 && user.DateOfBirth == date2)
            {
                check = true;
            }

            Assert.That(check, Is.True, "Incorrect information about user");
        }

        [Test]
        public void SocialDataSource_GetFriendsAndOnlineFriends_ShouldBeCorrectCalculateFriendsAndOnlineFriends()
        {
            var PathUsers = PathDirectory + @"GetFriendsAndOnlineFriends/users.json";
            var PathFriends = PathDirectory + @"GetFriendsAndOnlineFriends/friends.json";
            var PathMessages = PathDirectory + @"GetFriendsAndOnlineFriends/messages.json";
            var socialDataSource = new SocialDataSource(PathUsers, PathFriends, PathMessages);
            var userId = 4;
            var userFriends = socialDataSource.GetUserFriends(userId);
            var userOnlineFriends = socialDataSource.GetUserOnlineFriends(userFriends);

            Assert.That(userFriends.Count, Is.EqualTo(7), "Incorrect number of friends");
            Assert.That(userOnlineFriends.Count, Is.EqualTo(4), "Incorrect number of online friends");
        }

        [Test]
        public void SocialDataSource_GetSubscribers_ShouldBeCorrectCalculateSubscribers()
        {
            var PathUsers = PathDirectory + @"GetSubscribers/users.json";
            var PathFriends = PathDirectory + @"GetSubscribers/friends.json";
            var PathMessages = PathDirectory + @"GetSubscribers/messages.json";

            var socialDataSource = new SocialDataSource(PathUsers, PathFriends, PathMessages);
            var userId = 4;
            var userSubscribers = socialDataSource.GetUserSubscribers(userId);

            Assert.That(userSubscribers.Count, Is.EqualTo(3), "Incorrect number of subscribers");
        }

        [Test]
        public void SocialDataSource_GetFriendshipOffers_ShouldBeCorrectCalculateFriendshipOffers()
        {
            var PathUsers = PathDirectory + @"GetFriendshipOffers/users.json";
            var PathFriends = PathDirectory + @"GetFriendshipOffers/friends.json";
            var PathMessages = PathDirectory + @"GetFriendshipOffers/messages.json";

            var socialDataSource = new SocialDataSource(PathUsers, PathFriends,PathMessages);
            var user = socialDataSource.GetUserInformation("Elizabeth");
            var userFriendshipOffers = socialDataSource.GetUserFriendshipOffers(user);

            Assert.That(userFriendshipOffers.Count, Is.EqualTo(3), "Incorrect number of friendship îffers");
        }

        [Test]
        public void SocialDataSource_GetNews_ShouldBeCorrectCalculateNews()
        {
            var PathUsers = PathDirectory + @"GetNews/users.json";
            var PathFriends = PathDirectory + @"GetNews/friends.json";
            var PathMessages = PathDirectory + @"GetNews/messages.json";

            var socialDataSource = new SocialDataSource(PathUsers, PathFriends, PathMessages);
            var user = socialDataSource.GetUserInformation("Elizabeth");
            var userNews = socialDataSource.GetUserNews(user);

            Assert.That(userNews.Count, Is.EqualTo(4), "Incorrect number of news");
        }
    }
}