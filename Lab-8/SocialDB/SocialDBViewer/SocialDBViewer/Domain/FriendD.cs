namespace SocialDBViewer.Domain
{
    using System;

    public sealed class FriendD
    {
        public int FriendId { get; set; }

        public UserD FromUser { get; set; }

        public UserD ToUser { get; set; }

        public DateTime SendDate { get; set; }

        public int Status { get; set; }
    }
}
