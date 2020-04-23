namespace SocialDBViewer.DataAccess.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialSchema : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Friends",
                c => new
                    {
                        friendId = c.Int(nullable: false, identity: true),
                        sendDate = c.DateTime(nullable: false),
                        friendStatus = c.Int(nullable: false),
                        userFrom = c.Int(nullable: false),
                        userTo = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.friendId)
                .ForeignKey("dbo.Users", t => t.userFrom)
                .ForeignKey("dbo.Users", t => t.userTo, cascadeDelete: true)
                .Index(t => t.userFrom)
                .Index(t => t.userTo);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        userId = c.Int(nullable: false, identity: true),
                        dateOfBirth = c.DateTime(nullable: false),
                        gender = c.Int(nullable: false),
                        lastVisit = c.DateTime(nullable: false),
                        name = c.String(nullable: false, maxLength: 128),
                        isOnline = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.userId);
            
            CreateTable(
                "dbo.Likes",
                c => new
                    {
                        likeId = c.Int(nullable: false, identity: true),
                        messageId = c.Int(nullable: false),
                        userId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.likeId)
                .ForeignKey("dbo.Messages", t => t.messageId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.userId)
                .Index(t => t.messageId)
                .Index(t => t.userId);
            
            CreateTable(
                "dbo.Messages",
                c => new
                    {
                        messageId = c.Int(nullable: false, identity: true),
                        sendDate = c.DateTime(nullable: false),
                        messageText = c.String(),
                        authorId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.messageId)
                .ForeignKey("dbo.Users", t => t.authorId)
                .Index(t => t.authorId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Likes", "userId", "dbo.Users");
            DropForeignKey("dbo.Likes", "messageId", "dbo.Messages");
            DropForeignKey("dbo.Messages", "authorId", "dbo.Users");
            DropForeignKey("dbo.Friends", "userTo", "dbo.Users");
            DropForeignKey("dbo.Friends", "userFrom", "dbo.Users");
            DropIndex("dbo.Messages", new[] { "authorId" });
            DropIndex("dbo.Likes", new[] { "userId" });
            DropIndex("dbo.Likes", new[] { "messageId" });
            DropIndex("dbo.Friends", new[] { "userTo" });
            DropIndex("dbo.Friends", new[] { "userFrom" });
            DropTable("dbo.Messages");
            DropTable("dbo.Likes");
            DropTable("dbo.Users");
            DropTable("dbo.Friends");
        }
    }
}
