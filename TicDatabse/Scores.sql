CREATE TABLE [dbo].[Scores]
(
	[Id] INT NOT NULL PRIMARY KEY, 
    [userID] INT NOT NULL, 
    [wins] INT NOT NULL DEFAULT 0
)
