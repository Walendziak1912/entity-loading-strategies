CREATE TABLE Posts (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    UserId INT NOT NULL,
    Content NVARCHAR(1000) NOT NULL,
    Title NVARCHAR(200) NULL,
    ImageUrl NVARCHAR(500) NULL,
    LikesCount INT NOT NULL DEFAULT 0,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETDATE(),
    UpdatedAt DATETIME2 NOT NULL DEFAULT GETDATE(),
    CONSTRAINT FK_Posts_Users FOREIGN KEY (UserId) REFERENCES Users(Id) ON DELETE CASCADE
);

-- Indexy dla lepszej wydajności
CREATE INDEX IX_Posts_UserId ON Posts(UserId);
CREATE INDEX IX_Posts_CreatedAt ON Posts(CreatedAt);
CREATE INDEX IX_Posts_LikesCount ON Posts(LikesCount); 