use Social2
declare @messageId int

insert into dbo.Users (dateOfBirth, gender, lastVisit, isOnline, name) values ('1987-20-02', '0', '2020-20-02', 'False', 'Theresa Hebert')
insert into dbo.Users (dateOfBirth, gender, lastVisit, isOnline, name) values ('1987-20-02', '1', '2020-07-02', 'False', 'Veronica Campos')
insert into dbo.Users (dateOfBirth, gender, lastVisit, isOnline, name) values ('1987-20-02', '0', '2020-07-02', 'True', 'Diana Shelton')
insert into dbo.Users (dateOfBirth, gender, lastVisit, isOnline, name) values ('1987-20-02', '1', '2020-07-02', 'True', 'Jesse Burns')
insert into dbo.Users (dateOfBirth, gender, lastVisit, isOnline, name) values ('1987-20-02', '0', '2020-07-02', 'False', 'Hazel Oneill')
insert into dbo.Users (dateOfBirth, gender, lastVisit, isOnline, name) values ('1987-20-02', '1', '2020-07-02', 'False', 'Morgan Melendez')
insert into dbo.Users (dateOfBirth, gender, lastVisit, isOnline, name) values ('1987-20-02', '0' ,'2020-07-02', 'False', 'Victoria Contreras')
insert into dbo.Users (dateOfBirth, gender, lastVisit, isOnline, name) values ('1987-20-02', '1', '2020-07-02', 'False', 'Aleena Knapp')
insert into dbo.Users (dateOfBirth, gender, lastVisit, isOnline, name) values ('1987-20-02', '0', '2020-07-02', 'False', 'Irene Weiss')
insert into dbo.Users (dateOfBirth, gender, lastVisit, isOnline, name) values ('1987-20-02', '1', '2020-07-02', 'False', 'Tiana Watson')

insert into dbo.Friends (userFrom, userTo, friendStatus, sendDate) values ('1', '2', '2', '2019-27-09')
insert into dbo.Friends (userFrom, userTo, friendStatus, sendDate) values ('3', '1', '2', '2019-27-09')
insert into dbo.Friends (userFrom, userTo, friendStatus, sendDate) values ('1', '4', '0', '2019-27-09')
insert into dbo.Friends (userFrom, userTo, friendStatus, sendDate) values ('4', '1', '0', '2019-27-09')
insert into dbo.Friends (userFrom, userTo, friendStatus, sendDate) values ('1', '5', '1', '2019-27-09')
insert into dbo.Friends (userFrom, userTo, friendStatus, sendDate) values ('5', '1', '1', '2019-27-09')
insert into dbo.Friends (userFrom, userTo, friendStatus, sendDate) values ('1', '6', '3', '2019-27-09')
insert into dbo.Friends (userFrom, userTo, friendStatus, sendDate) values ('6', '1', '3', '2019-27-09')
insert into dbo.Friends (userFrom, userTo, friendStatus, sendDate) values ('7', '1', '0', '2019-27-09')
insert into dbo.Friends (userFrom, userTo, friendStatus, sendDate) values ('8', '1', '0', '2020-22-02')

insert into dbo.Messages(authorId, sendDate, messageText) values ('1', '2019-27-02', 'ERROR: NOT RELEVANT')
set @messageId = SCOPE_IDENTITY()

insert into dbo.Likes (messageId, userId) values (@messageId, '2')
insert into dbo.Likes (messageId, userId) values (@messageId, '3')

insert into dbo.Messages(authorId, sendDate, messageText) values ('2', '2019-27-02', 'ERROR: OUTDATED')
set @messageId = SCOPE_IDENTITY()

insert into dbo.Likes (messageId, userId) values (@messageId, '3')
insert into dbo.Likes (messageId, userId) values (@messageId, '4')
insert into dbo.Likes (messageId, userId) values (@messageId, '5')

insert into dbo.Messages(authorId, sendDate, messageText) values ('3', '2020-21-02', 'Success')
set @messageId = SCOPE_IDENTITY()

insert into dbo.Likes (messageId, userId) values (@messageId, '1')
insert into dbo.Likes (messageId, userId) values (@messageId, '2')
insert into dbo.Likes (messageId, userId) values (@messageId, '3')

insert into dbo.Messages(authorId, sendDate, messageText) values ('9', '2019-21-02', 'ERROR: NOT RELEVANT')
set @messageId = SCOPE_IDENTITY()

insert into dbo.Likes (messageId, userId) values (@messageId, '1')
insert into dbo.Likes (messageId, userId) values (@messageId, '2')
insert into dbo.Likes (messageId, userId) values (@messageId, '3')