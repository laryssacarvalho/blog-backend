using BlogApi.Application.Dtos;
using BlogApi.Domain.Entities;

namespace BlogApi.Application.Interfaces
{
    public interface IPostService
    {
        public Task<int> AddPost(string title, string content, int authorId);
        public Task EditPost(string title, string content, int postId, int authorId);
        public Task AddCommentToPost(string content, int postId, int userId);
        public Task<PostDto> GetPostById(int postId);
        public Task<IEnumerable<PostDto>> GetPostsByAuthor(int authorId);
        public Task<IEnumerable<PostDto>> GetPublishedPostsAsync();
        public Task<IEnumerable<PostDto>> GetPendingPostsAsync();
        public Task SubmitPostAsync(int postId, int authorId);
        public Task ApprovePostAsync(int postId);
        public Task RejectPostAsync(int postId, int editorId, string comment = null);
    }
}
