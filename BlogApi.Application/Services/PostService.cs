using BlogApi.Application.Dtos;
using BlogApi.Application.Interfaces;
using BlogApi.Domain.Entities;
using BlogApi.Domain.Enums;
using BlogApi.Domain.Exceptions;

namespace BlogApi.Application.Services
{
    public class PostService : IPostService
    {
        private readonly IPostRepository _postRepository;
        
        public PostService(IPostRepository postRepository)
        {
            _postRepository = postRepository;
        }

        public async Task<int> AddPost(string title, string content, int authorId)
        {
            var newPost = new Post(title, content, authorId);

            await _postRepository.AddPostAsync(newPost);
            
            return newPost.Id;
        }

        public async Task AddCommentToPost(string content, int postId, int userId)
        {
            var post = await _postRepository.GetPostByIdAsync(postId);

            if (post is null)
                throw new DomainException("Invalid post.");

            post.AddPublicComment(new Comment(content, userId));
            
            await _postRepository.UpdatePostAsync(post);
        }

        public async Task<IEnumerable<PostDto>> GetPostsByAuthor(int authorId)
        {
            var posts = await _postRepository.GetByAuthorIdAsync(authorId);
            var postDtos = posts.Select(p => new PostDto
            {
                Id = p.Id,
                Content = p.Content,
                PublishedAt = p.PublishedAt,
                Title = p.Title,
                Comments = p.Comments.Select(c => new CommentDto
                {
                    Content = c.Content,
                    IsRejection = c.IsRejection
                })
            });
            return postDtos;
        }

        public async Task<PostDto> GetPostById(int postId)
        {
            var post = await _postRepository.GetPostByIdAsync(postId);

            if (post is null)
                return null;

            var postDto = new PostDto
            {
                Id = post.Id,
                Content = post.Content,
                PublishedAt = post.PublishedAt,
                Title = post.Title,
                Comments = post.Comments.FindAll(c => !c.IsRejection).Select(c => new CommentDto
                {
                    Content = c.Content,
                    IsRejection = c.IsRejection
                })
            };
            return postDto;
        }

        public async Task EditPost(string newTitle, string newContent, int postId, int authorId)
        {
            var post = await _postRepository.GetPostByIdAsync(postId);

            if (post is null)
                throw new DomainException("Invalid post.");

            if (post.AuthorId != authorId)
                throw new DomainException("The post can be edited only by its author");

            post.Edit(newTitle, newContent);
            
            await _postRepository.UpdatePostAsync(post);
        }

        public async Task<IEnumerable<PostDto>> GetPublishedPostsAsync()
        {
            var posts = await _postRepository.GetPostsByStatusAsync(PostStatus.Published);
            var postDtos = posts.Select(p => new PostDto
            {
                Id = p.Id,
                Content = p.Content,
                PublishedAt = p.PublishedAt,
                Title = p.Title,
                Comments = p.Comments.FindAll(c => !c.IsRejection).Select(c => new CommentDto
                {
                    Content = c.Content,
                    IsRejection = c.IsRejection
                })
            });

            return postDtos;
        }

        public async Task SubmitPostAsync(int postId, int authorId)
        {
            var post = await _postRepository.GetPostByIdAsync(postId);

            if (post is null)
                throw new DomainException("Invalid post");

            if (post.AuthorId != authorId)
                throw new DomainException("The post can be submitted only by its author");

            post.Submit();

            await _postRepository.UpdatePostAsync(post);
        }

        public async Task<IEnumerable<PostDto>> GetPendingPostsAsync()
        {
            var posts = await _postRepository.GetPostsByStatusAsync(PostStatus.Pending);
            var postDtos = posts.Select(p => new PostDto
            {
                Id = p.Id,
                Content = p.Content,
                PublishedAt = p.PublishedAt,
                Title = p.Title,
                Comments = p.Comments.FindAll(c => !c.IsRejection).Select(c => new CommentDto
                {
                    Content = c.Content,
                    IsRejection = c.IsRejection
                })
            });

            return postDtos;
        }

        public async Task ApprovePostAsync(int postId)
        {
            var post = await _postRepository.GetPostByIdAsync(postId);

            if (post is null)
                throw new DomainException("Invalid post");

            post.Approve();

            await _postRepository.UpdatePostAsync(post);
        }

        public async Task RejectPostAsync(int postId, int editorId, string comment = null)
        {
            Comment rejectionComment = null;

            if (comment is not null)
                rejectionComment = new(comment, editorId, true);

            var post = await _postRepository.GetPostByIdAsync(postId);

            if (post is null)
                throw new DomainException("Invalid post.");

            post.Reject(rejectionComment);

            await _postRepository.UpdatePostAsync(post);
        }
    }
}
