namespace SocialDBViewer.Domain
{
    public sealed class LikeD
    {
        public int LikeId { get; set; }
        public UserD User { get; set; }

        public MessageD Message{ get; set; }
    }
}
