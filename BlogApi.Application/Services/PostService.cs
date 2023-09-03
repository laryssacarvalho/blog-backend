using BlogApi.Application.Interfaces;
using BlogApi.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace BlogApi.Application.Services
{
    public class PostService : IPostService
    {
        private readonly IRepository<Post> _postRepository;
        private readonly ILogger<PostService> _logger;

        public PostService(IRepository<Post> postRepository, ILogger<PostService> logger)
        {
            _postRepository = postRepository;
            _logger = logger;
        }

        public async Task<int> AddPost(string title, string content, int authorId)
        {
            var newPost = new Post(title, content, authorId);
            await _postRepository.AddAsync(newPost);
            
            return newPost.Id;
        }
        
    }
}
