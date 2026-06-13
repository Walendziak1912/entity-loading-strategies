-- Wypełnienie tabeli Users danymi testowymi
INSERT INTO Users (Username, Email, FirstName, LastName, Bio, ProfilePictureUrl) VALUES
('john_doe', 'john.doe@example.com', 'John', 'Doe', 'Software developer and tech enthusiast', 'https://example.com/profiles/john.jpg'),
('jane_smith', 'jane.smith@example.com', 'Jane', 'Smith', 'Digital marketing specialist', 'https://example.com/profiles/jane.jpg'),
('mike_wilson', 'mike.wilson@example.com', 'Mike', 'Wilson', 'Photographer and travel blogger', 'https://example.com/profiles/mike.jpg'),
('sarah_jones', 'sarah.jones@example.com', 'Sarah', 'Jones', 'Fitness trainer and nutrition expert', 'https://example.com/profiles/sarah.jpg'),
('david_brown', 'david.brown@example.com', 'David', 'Brown', 'Music producer and DJ', 'https://example.com/profiles/david.jpg'),
('emma_davis', 'emma.davis@example.com', 'Emma', 'Davis', 'Graphic designer and artist', 'https://example.com/profiles/emma.jpg'),
('alex_taylor', 'alex.taylor@example.com', 'Alex', 'Taylor', 'Chef and food blogger', 'https://example.com/profiles/alex.jpg'),
('lisa_garcia', 'lisa.garcia@example.com', 'Lisa', 'Garcia', 'Teacher and education advocate', 'https://example.com/profiles/lisa.jpg'),
('tom_anderson', 'tom.anderson@example.com', 'Tom', 'Anderson', 'Entrepreneur and startup founder', 'https://example.com/profiles/tom.jpg'),
('anna_white', 'anna.white@example.com', 'Anna', 'White', 'Doctor and health advocate', 'https://example.com/profiles/anna.jpg');

-- Wypełnienie tabeli Posts danymi testowymi
INSERT INTO Posts (UserId, Content, Title, ImageUrl, LikesCount) VALUES
(1, 'Just finished building an amazing new web application! The latest technologies are incredible.', 'New Web App Complete', 'https://example.com/images/webapp.jpg', 15),
(2, 'Excited to share some digital marketing tips that have been working wonders for my clients.', 'Marketing Tips & Tricks', 'https://example.com/images/marketing.jpg', 23),
(3, 'Captured this beautiful sunset during my trip to the mountains. Nature is truly breathtaking!', 'Mountain Sunset', 'https://example.com/images/sunset.jpg', 45),
(4, 'New workout routine is paying off! Here are some exercises that have transformed my fitness journey.', 'Fitness Transformation', 'https://example.com/images/fitness.jpg', 67),
(5, 'Working on a new track that I think you all will love. Music production is my passion!', 'New Music Coming Soon', 'https://example.com/images/music.jpg', 89),
(6, 'Just completed a new design project. Creativity flows when you love what you do!', 'Design Project Complete', 'https://example.com/images/design.jpg', 34),
(7, 'Made this delicious pasta dish today. Cooking is therapy for the soul!', 'Homemade Pasta', 'https://example.com/images/pasta.jpg', 56),
(8, 'Teaching is not just a job, it''s a calling. Today my students amazed me with their creativity.', 'Teaching Moments', 'https://example.com/images/teaching.jpg', 28),
(9, 'Startup life is challenging but rewarding. Here''s what I learned this week.', 'Startup Lessons', 'https://example.com/images/startup.jpg', 42),
(10, 'Health is wealth! Remember to take care of yourself and your loved ones.', 'Health Reminder', 'https://example.com/images/health.jpg', 78);

-- Wypełnienie tabeli Comments danymi testowymi
INSERT INTO Comments (PostId, UserId, Content, LikesCount) VALUES
(1, 2, 'This looks amazing! What technologies did you use?', 5),
(1, 3, 'Great work! Would love to see more details about the implementation.', 3),
(1, 4, 'Inspiring! Makes me want to learn web development.', 7),
(2, 1, 'These tips are gold! Thanks for sharing your expertise.', 12),
(2, 5, 'I''ve been using some of these strategies and they really work!', 8),
(3, 6, 'Stunning photo! The colors are absolutely beautiful.', 15),
(3, 7, 'This makes me want to plan a mountain trip immediately!', 9),
(4, 8, 'Your transformation is incredible! Keep up the great work.', 20),
(4, 9, 'What''s your secret? I need to get back in shape too!', 11),
(5, 10, 'Can''t wait to hear the new track! Your music is always amazing.', 25),
(5, 1, 'Looking forward to this! Your previous work was fantastic.', 18),
(6, 2, 'The design looks so clean and modern. Great job!', 14),
(6, 3, 'What software do you use for your designs?', 6),
(7, 4, 'That pasta looks delicious! Can you share the recipe?', 22),
(7, 5, 'I love cooking too! There''s something therapeutic about it.', 13),
(8, 6, 'Teachers like you make such a difference in students'' lives.', 19),
(8, 7, 'Education is the foundation of everything. Thank you for your dedication!', 16),
(9, 8, 'Startup life is indeed challenging. Your insights are valuable!', 10),
(9, 9, 'Been there! The journey is tough but worth it.', 8),
(10, 1, 'So true! Health should always be a priority.', 30),
(10, 2, 'Thank you for the reminder. We all need to hear this sometimes.', 17); 