namespace BlogApi.Application.Interfaces
{
    public interface IPostService
    {
        public Task<int> AddPost(string title, string content, int authorId);
    }
}
