namespace Generator
{
    using System;
    using System.IO;
    using System.Collections.Generic;
    using System.Linq;
    using Social;
    using Social.Models;
    using System.Text.Json;

    class SocialGenerator
    {
        private List<User> _users;
        private List<Friend> _friends;
        private List<Message> _messages;
       
        public void SetGeneration(string pathUsers, string pathFriends, string pathMessages)
        {
            var socialDataSource = new SocialDataSource(pathUsers, pathFriends, pathMessages);

            _users = socialDataSource._users;
            _friends = socialDataSource._friends;
            _messages = socialDataSource._messages;

            var numberOfUsers = 100;
            var numberOfFriends = 1000;
            var numberOfMessages = 200;

            SetUsers(pathUsers, numberOfUsers);
            SetFriends(pathFriends, numberOfFriends);
            SetMessages(pathMessages, numberOfMessages);
        }

        public void SetUsers(string pathUsers, int numberOfUsers)
        {
            var availableNames = GetAvailableNames();

            while (numberOfUsers - _users.Count > 0) 
            {
                var rnd = new Random();

                //name
                var indexName = rnd.Next(availableNames.Count);
                var name = availableNames[indexName];
                //gender
                char[] ch = new char[] { 'e', 'a', 'y' };
                var gender = (ch.Contains(name.Last())) ? 1 : 0;
                //id
                var id = _users.Count + 1;
                //birthday 
                var year = rnd.Next(1950, 2006);
                var month = rnd.Next(1, 13);
                var day = rnd.Next(1, 29);
                var birthday = new DateTime(year, month, day);
                //lastVisit
                year = rnd.Next(2017, 2020);
                month = rnd.Next(1, 13);
                day = rnd.Next(1, 29);
                var lastVisit = new DateTime(year, month, day);
                //online
                var online = (rnd.Next(2) == 1) ? true : false;

                var user = new User()
                {
                    DateOfBirth = birthday,
                    Gender = gender,
                    LastVisit = lastVisit,
                    Name = name,
                    Online = online,
                    UserId = id
                };

                _users.Add(user);
            }

            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };

            var json = JsonSerializer.Serialize<List<User>>(_users, options);

            using (FileStream fs = new FileStream(pathUsers, FileMode.OpenOrCreate))
            {
                byte[] array = System.Text.Encoding.Default.GetBytes(json);
                fs.Write(array, 0, array.Length);
                Console.WriteLine("The file users.json was generated");
            }
        }

        public void SetFriends(string pathFriends, int numberOfFriends)
        {

            while (numberOfFriends - _friends.Count > 0)
            {
                var rnd = new Random();

                //fromUserId
                var fromUserId = rnd.Next(1, _users.Count + 1);
                //toUserId
                var id = rnd.Next(1, _users.Count + 1);

                while (id == fromUserId)
                {
                    id = rnd.Next(1, _users.Count + 1);
                }

                var toUserId = id;

                //status
                var status = rnd.Next(4);

                //sendDate
                var year = rnd.Next(2018, 2020);
                var month = rnd.Next(1, 13);
                var day = rnd.Next(1, 29);
                var sendDate = new DateTime(year, month, day);

                var friend = new Friend()
                {
                    FromUserId = fromUserId,
                    SendDate = sendDate,
                    Status = status,
                    ToUserId = toUserId

                };

                _friends.Add(friend);
            }

            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };

            var json = JsonSerializer.Serialize<List<Friend>>(_friends, options);

            using (FileStream fs = new FileStream(pathFriends, FileMode.OpenOrCreate))
            {
                byte[] array = System.Text.Encoding.Default.GetBytes(json);
                fs.Write(array, 0, array.Length);
                Console.WriteLine("The file friends.json was generated");
            }
        }

        public void SetMessages(string pathMessages, int numberOfMessages)
        {

            while (numberOfMessages - _messages.Count > 0)
            {
                var rnd = new Random();

                //authorId
                var authorId = rnd.Next(1, _users.Count + 1);

                //likes
                var numberOfLikes = rnd.Next(1, _users.Count + 1);
                var likes = new List<int>();

                for(var i=0; i< numberOfLikes; i++) 
                {
                    var id = rnd.Next(1, _users.Count + 1);

                    while (likes.Contains(id))
                    {
                        id = rnd.Next(1, _users.Count + 1);
                    }

                    likes.Add(id);
                }

                //idMessage
                var idMessage = _messages.Count + 1;

                //text
                var lengthText = rnd.Next(300);
                string text = String.Empty;

                for (var i = 0; i < numberOfLikes; i++)
                {
                    var ch = rnd.Next(97,123);
                    var spacing = rnd.Next(50);

                    text = (spacing < 15) ? text + " " : text + Convert.ToChar(ch);
                }

                text.Trim();

                //sendDate
                var year = rnd.Next(2018, 2020);
                var month = rnd.Next(1, 13);
                var day = rnd.Next(1, 29);
                var sendDate = new DateTime(year, month, day);

                var message = new Message()
                {
                    AuthorId = authorId,
                    Likes = likes,
                    MessageId = idMessage,
                    SendDate = sendDate,
                    Text = text
                };
                
                _messages.Add(message);
            }
            
            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };

            var json = JsonSerializer.Serialize<List<Message>>(_messages, options);

            using (FileStream fs = new FileStream(pathMessages, FileMode.OpenOrCreate))
            {
                byte[] array = System.Text.Encoding.Default.GetBytes(json);
                fs.Write(array, 0, array.Length);
                Console.WriteLine("The file messages.json was generated");
            }
        }

        public List<string> GetAvailableNames() 
        {
            var path = @"../../../Data/Names.txt";
            var allNamesInFile = new List<string>();

            using (StreamReader sr = new StreamReader(path, System.Text.Encoding.Default))
            {
                string line;

                while ((line = sr.ReadLine()) != null)
                {
                    allNamesInFile.Add(line.Trim());
                }

            };

            var uniqueNamesInFile = allNamesInFile.Distinct().ToList();
            var namesInCollection = new List<string>();

            foreach (var user in _users)
            {
                namesInCollection.Add(user.Name);
            }

            return uniqueNamesInFile.Except(namesInCollection).ToList();
        }

    }
}
