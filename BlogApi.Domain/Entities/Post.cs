using BlogApi.Domain.Enums;

namespace BlogApi.Domain.Entities
{
    public class Post : Entity
    {
        public string Title { get; private set; }
        public string Content { get; private set; }
        public DateTime? PublishedAt { get; private set; }
        public int AuthorId { get; private set; }
        public PostStatus Status { get; set; }

        //public List<Comment> Comments { get; private set; }
        public User Author { get; private set; }

        public Post(string title, string content, int authorId)
        {
            Title = title;
            Content = content;
            AuthorId = authorId;
            Status = PostStatus.Created;
            //Comments = new();
        }

        //public void AddComment(Comment comment) => Comments.Add(comment);

        public void Submit() => Status = PostStatus.Pending;

        public void Approve()
        {
            Status = PostStatus.Published;
            PublishedAt = DateTime.UtcNow;
        }

        public void Reject()
        {
            Status = PostStatus.Rejected;
        }

        public void Edit(string title, string content)
        {
            if (Status == PostStatus.Pending || Status == PostStatus.Published)
                throw new Exception();

            Title = title;
            Content = content;
        }
    }
}
