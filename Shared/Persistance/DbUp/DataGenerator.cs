using Bogus;
using Microsoft.Data.SqlClient;

namespace PersistanceDbUp
{
    public class DataGenerator
    {
        private readonly string _connectionString;

        public DataGenerator(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task GenerateTestDataAsync()
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            // Generuj użytkowników
            var userFaker = new Faker<User>()
                .RuleFor(u => u.Username, f => f.Internet.UserName())
                .RuleFor(u => u.Email, f => f.Internet.Email())
                .RuleFor(u => u.FirstName, f => f.Name.FirstName())
                .RuleFor(u => u.LastName, f => f.Name.LastName())
                .RuleFor(u => u.Bio, f => f.Lorem.Sentence())
                .RuleFor(u => u.ProfilePictureUrl, f => f.Internet.Avatar());

            var users = userFaker.Generate(50);

            // Wstaw użytkowników
            foreach (var user in users)
            {
                var insertUserQuery = @"
                    INSERT INTO Users (Username, Email, FirstName, LastName, Bio, ProfilePictureUrl) 
                    VALUES (@Username, @Email, @FirstName, @LastName, @Bio, @ProfilePictureUrl);
                    SELECT SCOPE_IDENTITY();";

                using var command = new SqlCommand(insertUserQuery, connection);
                command.Parameters.AddWithValue("@Username", user.Username);
                command.Parameters.AddWithValue("@Email", user.Email);
                command.Parameters.AddWithValue("@FirstName", user.FirstName);
                command.Parameters.AddWithValue("@LastName", user.LastName);
                command.Parameters.AddWithValue("@Bio", user.Bio);
                command.Parameters.AddWithValue("@ProfilePictureUrl", user.ProfilePictureUrl);

                var userId = Convert.ToInt32(await command.ExecuteScalarAsync());
                user.Id = userId;
            }

            // Generuj posty
            var postFaker = new Faker<Post>()
                .RuleFor(p => p.UserId, f => f.PickRandom(users).Id)
                .RuleFor(p => p.Content, f => f.Lorem.Paragraph())
                .RuleFor(p => p.Title, f => f.Lorem.Sentence())
                .RuleFor(p => p.ImageUrl, f => f.Image.PicsumUrl())
                .RuleFor(p => p.LikesCount, f => f.Random.Int(0, 100));

            var posts = postFaker.Generate(200);

            // Wstaw posty
            foreach (var post in posts)
            {
                var insertPostQuery = @"
                    INSERT INTO Posts (UserId, Content, Title, ImageUrl, LikesCount) 
                    VALUES (@UserId, @Content, @Title, @ImageUrl, @LikesCount);
                    SELECT SCOPE_IDENTITY();";

                using var command = new SqlCommand(insertPostQuery, connection);
                command.Parameters.AddWithValue("@UserId", post.UserId);
                command.Parameters.AddWithValue("@Content", post.Content);
                command.Parameters.AddWithValue("@Title", post.Title);
                command.Parameters.AddWithValue("@ImageUrl", post.ImageUrl);
                command.Parameters.AddWithValue("@LikesCount", post.LikesCount);

                var postId = Convert.ToInt32(await command.ExecuteScalarAsync());
                post.Id = postId;
            }

            // Generuj komentarze
            var commentFaker = new Faker<Comment>()
                .RuleFor(c => c.PostId, f => f.PickRandom(posts).Id)
                .RuleFor(c => c.UserId, f => f.PickRandom(users).Id)
                .RuleFor(c => c.Content, f => f.Lorem.Sentence())
                .RuleFor(c => c.LikesCount, f => f.Random.Int(0, 50));

            var comments = commentFaker.Generate(500);

            // Wstaw komentarze
            foreach (var comment in comments)
            {
                var insertCommentQuery = @"
                    INSERT INTO Comments (PostId, UserId, Content, LikesCount) 
                    VALUES (@PostId, @UserId, @Content, @LikesCount)";

                using var command = new SqlCommand(insertCommentQuery, connection);
                command.Parameters.AddWithValue("@PostId", comment.PostId);
                command.Parameters.AddWithValue("@UserId", comment.UserId);
                command.Parameters.AddWithValue("@Content", comment.Content);
                command.Parameters.AddWithValue("@LikesCount", comment.LikesCount);

                await command.ExecuteNonQueryAsync();
            }
        }

        private class User
        {
            public int Id { get; set; }
            public string Username { get; set; } = string.Empty;
            public string Email { get; set; } = string.Empty;
            public string FirstName { get; set; } = string.Empty;
            public string LastName { get; set; } = string.Empty;
            public string Bio { get; set; } = string.Empty;
            public string ProfilePictureUrl { get; set; } = string.Empty;
        }

        private class Post
        {
            public int Id { get; set; }
            public int UserId { get; set; }
            public string Content { get; set; } = string.Empty;
            public string Title { get; set; } = string.Empty;
            public string ImageUrl { get; set; } = string.Empty;
            public int LikesCount { get; set; }
        }

        private class Comment
        {
            public int PostId { get; set; }
            public int UserId { get; set; }
            public string Content { get; set; } = string.Empty;
            public int LikesCount { get; set; }
        }
    }
} 