namespace Generator
{
    using System;

    class Program
    {
        private const string PathDirectory = @"../../../../Social/Data/";
        private const string PathUsers = PathDirectory + @"users.json";
        private const string PathFriends = PathDirectory + @"friends.json";
        private const string PathMessages = PathDirectory + @"messages.json";

        static void Main(string[] args)
        {
            var generator = new SocialGenerator();
            generator.SetGeneration(PathUsers, PathFriends, PathMessages);

        }
    }
}
