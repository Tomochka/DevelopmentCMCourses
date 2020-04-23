namespace SocialDBViewer.Domain
{
    using System;
    using System.Collections.Generic;

    public sealed class MessageD
    {
        public List<int> Likes { get; set; }

        public int MessageId { get; set; }

        public DateTime SendDate { get; set; }

        public string Text { get; set; }

        public UserD User{ get; set; }
    }
}
