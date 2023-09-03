namespace BlogApi.Domain.Entities
{
    public class Comment
    {
        public string Content { get; private set; }
        public int UserId { get; private set; }
        public bool IsRejection { get; private set; }
        public int PostId { get; set; }
        public Post Post { get; private set; }

        public Comment(string content, int userId, bool isRejection = false)
        {
            Content = content;
            UserId = userId;
            IsRejection = isRejection;
        }
    }
}
