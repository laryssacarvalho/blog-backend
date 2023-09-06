using BlogApi.Application.Interfaces;
using BlogApi.Domain.Entities;
using BlogApi.Domain.Enums;
using BlogApi.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace BlogApi.Infrastructure.Repositories
{
    public class PostRepository : Repository<Post>, IPostRepository
    {
        public PostRepository(BlogDbContext dbContext) : base(dbContext)
        {
        }
        public async Task UpdatePostAsync(Post post) => await UpdateAsync(post);

        public async Task AddPostAsync(Post post) => await AddAsync(post);

        public async Task<IEnumerable<Post>> GetPostsByStatusAsync(PostStatus status)
        {
            return await FindAsync(p => p.Status == status, i => i.Include(p => p.Comments));
        }
        public async Task<Post> GetPostByIdAsync(int id)
        {
            return await FirstOrDefaultAsync(p => p.Id == id, i => i.Include(p => p.Comments));
        }

        public async Task<IEnumerable<Post>> GetByAuthorIdAsync(int authorId)
        {
            return await FindAsync(p => p.AuthorId == authorId, i => i.Include(p => p.Comments));
        }
    }
}
