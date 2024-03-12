CREATE TABLE Users(
 Id SERIAL Primary Key,
	Money DECIMAL DEFAULT  100,
	UserName VARCHAR(255) NOT NULL
)
CREATE TABLE MatchHistory(
    Id SERIAL PRIMARY KEY,
	UserOneId INT REFERENCES USERS(Id) NOT NULL,
	UserTwoId INT REFERENCES USERS(Id) NOT NULL,
	WinnderId INT REFERENCES USERS(Id)
)
CREATE TABLE GameTransactions(
 FromUserId INT REFERENCES USERS(Id) NOT NULL,
	ToUserId INT REFERENCES USERS(Id) NOT NULL,
	TransferAmount DECIMAL NOT NULL
)
DROP TABLE USERS,GameTransactions,MatchHistory

INSERT INTO USERS (UserName)VALUES(NULL)
SELECT * FROM USERS
DELETE FROM USERS WHERE USERNAME = NULL