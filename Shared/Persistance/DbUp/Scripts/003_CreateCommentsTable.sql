CREATE TABLE Comments (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    PostId INT NOT NULL,
    UserId INT NOT NULL,
    Content NVARCHAR(500) NOT NULL,
    LikesCount INT NOT NULL DEFAULT 0,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETDATE(),
    UpdatedAt DATETIME2 NOT NULL DEFAULT GETDATE(),
    CONSTRAINT FK_Comments_Posts FOREIGN KEY (PostId) REFERENCES Posts(Id) ON DELETE CASCADE,
    CONSTRAINT FK_Comments_Users FOREIGN KEY (UserId) REFERENCES Users(Id) ON DELETE NO ACTION
);

-- Indexy dla lepszej wydajności
CREATE INDEX IX_Comments_PostId ON Comments(PostId);
CREATE INDEX IX_Comments_UserId ON Comments(UserId);
CREATE INDEX IX_Comments_CreatedAt ON Comments(CreatedAt); 