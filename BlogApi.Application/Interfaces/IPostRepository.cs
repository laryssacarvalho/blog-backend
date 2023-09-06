using BlogApi.Domain.Entities;
using BlogApi.Domain.Enums;

namespace BlogApi.Application.Interfaces
{
    public interface IPostRepository
    {
        public Task<IEnumerable<Post>> GetPostsByStatusAsync(PostStatus status);
        public Task<IEnumerable<Post>> GetByAuthorIdAsync(int authorId);
        public Task<Post> GetPostByIdAsync(int id);
        public Task AddPostAsync(Post post);
        public Task UpdatePostAsync(Post post);
    }
}
