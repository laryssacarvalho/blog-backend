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
        //published
        //public IEnumerable<Post> GetAllPosts() => _postRepository.GetAll();
        
        //ADD COMMENT TO POST
        //GET WRITER POSTS
        //CREATE POST (WRITER)
        //EDIT POST (WRITER) (NOT PUBLISHED/SUBMITTED)
        //SUBMIT POST



    }
}
