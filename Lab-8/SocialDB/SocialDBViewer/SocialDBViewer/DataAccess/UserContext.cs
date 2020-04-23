namespace SocialDBViewer
{
    using System.Collections.Generic;
    using SocialDBViewer.Models;
    using SocialDBViewer.Domain;

    public class UserContext
    {
        public UserD User { get; set; }

        public List<UserInformation> Friends { get; set; }

        public List<UserInformation> OnlineFriends { get; set; }

        public List<UserInformation> FriendshipOffers { get; set; }

        public List<UserInformation> Subscribers { get; set; }

        public List<News> News { get; set; }
    }
}
