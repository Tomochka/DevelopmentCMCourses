namespace SocialDBViewer.DataAccess.EntityFramework
{
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity;
    using SocialDBViewer.Domain;

    class DataContext : DbContext
    {
        public DataContext() : this("DefaultConnectionString")
        {
        }

        public DataContext(string nameOrConnectionString) : base(nameOrConnectionString)
        {
        }

        public DbSet<UserD> Users { get; set; }

        public DbSet<FriendD> Friends { get; set; }

          public DbSet<MessageD> Messages { get; set; }

        public DbSet<LikeD> Likes { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //Users
            var usersModel = modelBuilder.Entity<UserD>();
            
            usersModel
                .ToTable("Users")
                .HasKey(x => x.UserId)
                .Property(x => x.UserId)
                .HasColumnName("userId")
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            usersModel.Property(x => x.Name).HasColumnName("name").HasMaxLength(128).IsRequired();
            usersModel.Property(x => x.DateOfBirth).HasColumnName("dateOfBirth");
            usersModel.Property(x => x.Gender).HasColumnName("gender");
            usersModel.Property(x => x.LastVisit).HasColumnName("lastVisit");
            usersModel.Property(x => x.Online).HasColumnName("isOnline").IsRequired();

            //Friends
            var friendsModel = modelBuilder.Entity<FriendD>();

            friendsModel
                .ToTable("Friends")
                .HasKey(x => x.FriendId)
                .Property(x => x.FriendId)
                .HasColumnName("friendId")
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            friendsModel.Property(x => x.SendDate).HasColumnName("sendDate");
            friendsModel.Property(x => x.Status).HasColumnName("friendStatus");

            friendsModel
                .HasRequired(s => s.FromUser)
                .WithMany()
                .Map(x => x.MapKey("userFrom"))
                .WillCascadeOnDelete(false);

            friendsModel
             .HasRequired(s => s.ToUser)
             .WithMany()
             .Map(x => x.MapKey("userTo"))
             .WillCascadeOnDelete(true);

            //Messages
            var messagesModel = modelBuilder.Entity<MessageD>();
             messagesModel
                 .ToTable("Messages")
                 .HasKey(x => x.MessageId)
                 .Property(x => x.MessageId)
                 .HasColumnName("messageId")
                 .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

             messagesModel.Property(x => x.SendDate).HasColumnName("sendDate").IsRequired();
             messagesModel.Property(x => x.Text).HasColumnName("messageText");

             messagesModel
                .HasRequired(s => s.User)
                .WithMany()
                .Map(x => x.MapKey("authorId"))
                .WillCascadeOnDelete(false);

            //Likes
              var likesModel = modelBuilder.Entity<LikeD>();

              likesModel
                  .ToTable("Likes").HasKey(x => x.LikeId)
                 .Property(x => x.LikeId)
                 .HasColumnName("likeId")
                 .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            likesModel
               .HasRequired(s => s.User)
                .WithMany()
                .Map(x => x.MapKey("userId"))
                .WillCascadeOnDelete(false);

            likesModel
               .HasRequired(s => s.Message)
                .WithMany()
                .Map(x => x.MapKey("messageId"))
                .WillCascadeOnDelete(true);

        }
    }

}

