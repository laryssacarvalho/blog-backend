namespace BlogApi.Application.Dtos
{
    public class PostDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime? PublishedAt { get; set; }
        public IEnumerable<CommentDto> Comments { get; set; }
    }

    public class CommentDto
    {
        public string Content { get; set; }        
        public bool IsRejection { get; set; }        
    }
}
