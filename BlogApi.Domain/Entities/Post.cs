using BlogApi.Domain.Enums;
using BlogApi.Domain.Exceptions;

namespace BlogApi.Domain.Entities
{
    public class Post : Entity
    {
        public string Title { get; private set; }
        public string Content { get; private set; }
        public DateTime? PublishedAt { get; private set; }
        public int AuthorId { get; private set; }
        public PostStatus Status { get; private set; }

        public List<Comment> Comments { get; private set; }
        public User Author { get; private set; }

        public Post(string title, string content, int authorId)
        {
            ValidateTitleAndContent(title, content);

            Title = title;
            Content = content;
            AuthorId = authorId;
            Status = PostStatus.Created;
            Comments = new();
        }

        private void ValidateTitleAndContent(string title, string content)
        {
            if (title is null)
                throw new DomainException("Title cannot be empty");

            if (content is null)
                throw new DomainException("Content cannot be empty");
        }
        public void Edit(string newTitle, string newContent)
        {
            ValidateTitleAndContent(newTitle, newContent);

            if (Status == PostStatus.Published || Status == PostStatus.Pending)
                throw new DomainException($"It is not possible to edit this post");

            Title = newTitle;
            Content = newContent;
        }

        public void AddPublicComment(string comment, int userId)
        {            
            if (Status != PostStatus.Published)
                throw new DomainException("Comments can be added only to published posts.");

            Comments.Add(new Comment(comment, userId));
        }

        public void Submit() 
        {
            if (Status == PostStatus.Published)
                throw new DomainException("This post is already published.");

            Status = PostStatus.Pending;
        }

        public void Approve()
        {
            if (Status != PostStatus.Pending)
                throw new DomainException("This post was not submitted yet.");

            Status = PostStatus.Published;
            PublishedAt = DateTime.UtcNow;
        }

        public void Reject(int userId, string comment = null) 
        {
            if (Status != PostStatus.Pending)
                throw new DomainException("This post was not submitted yet.");

            if (comment is not null)
                Comments.Add(new Comment(comment, userId, true));

            Status = PostStatus.Rejected;
        }

    }
}
