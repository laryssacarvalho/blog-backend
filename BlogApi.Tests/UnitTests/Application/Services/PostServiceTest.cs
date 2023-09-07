using BlogApi.Application.Interfaces;
using BlogApi.Application.Services;
using BlogApi.Domain.Entities;
using BlogApi.Domain.Enums;
using BlogApi.Domain.Exceptions;
using Moq;
using Moq.AutoMock;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApi.Tests.UnitTests.Application.Services
{
    public class PostServiceTest
    {
        private readonly AutoMocker _mocker;
        private readonly PostService _sut;
        public PostServiceTest()
        {
            _mocker = new();
            _sut = _mocker.CreateInstance<PostService>();
        }

        [Fact]
        public async Task AddPost_ShouldCallRepository()
        {
            //Act
            await _sut.AddPost("title", "content", 1);

            //Arrange
            _mocker.GetMock<IPostRepository>()
                .Verify(x => x.AddPostAsync(It.Is<Post>(p => p.Title == "title" && 
                    p.Content == "content" && p.AuthorId == 1)), Times.Once);
        }

        [Fact]
        public async Task AddCommentToPost_ShouldThrowDomainException_WhenPostDoesNotExist()
        {
            //Act
            Task act() => _sut.AddCommentToPost("comment", 1, 1);
            
            //Assert
            await Assert.ThrowsAsync<DomainException>(act);            
        }

        [Fact]
        public async Task AddCommentToPost_ShouldAddComment_WhenPostExists()
        {
            //Arrange
            var post = new Post("title", "content", 1);
            post.Submit();
            post.Approve();

            _mocker.GetMock<IPostRepository>()
                .Setup(x => x.GetPostByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(post);

            //Act
            await _sut.AddCommentToPost("comment", 1, 1);

            //Assert
            _mocker.GetMock<IPostRepository>()
                .Verify(x => x.UpdatePostAsync(It.Is<Post>(p => p.Comments.Count == 1)), Times.Once);
        }

        [Fact]
        public async Task GetPostsByAuthor_ShouldReturnPostsByAuthor()
        {
            //Arrange
            var posts = new List<Post>
            {
                new("title", "content", 1)
            };

            _mocker.GetMock<IPostRepository>()
                .Setup(x => x.GetByAuthorIdAsync(It.IsAny<int>()))
                .ReturnsAsync(posts);

            //Act
            var result = await _sut.GetPostsByAuthor(1);

            //Assert
            Assert.Single(result);
            _mocker.GetMock<IPostRepository>()
                .Verify(x => x.GetByAuthorIdAsync(It.Is<int>(i => i == 1)), Times.Once);
        }

        [Fact]
        public async Task GetPostById_ShouldReturnNull_WhenPostDoesNotExist()
        {            
            //Act
            var result = await _sut.GetPostById(1);

            //Assert
            Assert.Null(result);
            _mocker.GetMock<IPostRepository>()
                .Verify(x => x.GetPostByIdAsync(It.Is<int>(i => i == 1)), Times.Once);
        }

        [Fact]
        public async Task GetPostById_ShouldReturnPost_WhenPostExists()
        {
            //Arrange
            _mocker.GetMock<IPostRepository>()
                .Setup(x => x.GetPostByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(new Post("title", "content", 1));

            //Act
            var result = await _sut.GetPostById(1);

            //Assert
            Assert.NotNull(result);
            _mocker.GetMock<IPostRepository>()
                .Verify(x => x.GetPostByIdAsync(It.Is<int>(i => i == 1)), Times.Once);
        }

        [Fact]
        public async Task EditPost_ShouldThrowDomainException_WhenPostDoesNotExist()
        {            
            //Act
            Task act() => _sut.EditPost("new title", "new content", 1, 1);

            //Assert                        
            await Assert.ThrowsAsync<DomainException>(act);
            _mocker.GetMock<IPostRepository>()
                .Verify(x => x.UpdatePostAsync(It.IsAny<Post>()), Times.Never);
        }

        [Fact]
        public async Task EditPost_ShouldThrowDomainException_WhenUserIsNotTheAuthor()
        {
            //Arrange
            var post = new Post("title", "content", 1);
            
            _mocker.GetMock<IPostRepository>()
                .Setup(x => x.GetPostByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(post);

            //Act
            Task act() => _sut.EditPost("new title", "new content", 1, 2);

            //Assert                        
            await Assert.ThrowsAsync<DomainException>(act);

            _mocker.GetMock<IPostRepository>()
                .Verify(x => x.UpdatePostAsync(It.IsAny<Post>()), Times.Never);
        }

        [Fact]
        public async Task EditPost_ShouldUpdatePost_WhenPostExists()
        {
            //Arrange
            var post = new Post("title", "content", 1);

            _mocker.GetMock<IPostRepository>()
                .Setup(x => x.GetPostByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(post);

            //Act
            await _sut.EditPost("new title", "new content", 1, 1);

            //Assert                                    
            _mocker.GetMock<IPostRepository>()
                .Verify(x => x.UpdatePostAsync(It.Is<Post>(p => p.Title == "new title" && p.Content == "new content")), Times.Once);
        }

        [Fact]
        public async Task GetPublishedPostsAsync_ShouldReturnPublishedPosts()
        {
            //Arrange
            var post = new Post("title", "content", 1);
        
            _mocker.GetMock<IPostRepository>()
                .Setup(x => x.GetPostsByStatusAsync(It.IsAny<PostStatus>()))
                .ReturnsAsync(new List<Post> { post });

            //Act
            var result = await _sut.GetPublishedPostsAsync();

            //Assert                                    
            Assert.Single(result);
            _mocker.GetMock<IPostRepository>()
                .Verify(x => x.GetPostsByStatusAsync(It.Is<PostStatus>(p => p == PostStatus.Published)), Times.Once);
        }

        [Fact]
        public async Task SubmitPostAsync_ShouldThrowDomainException_WhenPostDoesNotExist()
        {            
            //Act
            Task act() => _sut.SubmitPostAsync(1, 1);

            //Assert
            await Assert.ThrowsAsync<DomainException>(act);
            
            _mocker.GetMock<IPostRepository>()
                .Verify(x => x.UpdatePostAsync(It.IsAny<Post>()), Times.Never);
        }

        [Fact]
        public async Task SubmitPostAsync_ShouldThrowDomainException_WhenUserIsNotTheAuthor()
        {
            //Arrange
            var post = new Post("title", "content", 1);

            _mocker.GetMock<IPostRepository>()
                .Setup(x => x.GetPostByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(post);

            //Act
            Task act() => _sut.SubmitPostAsync(1, 2);

            //Assert                        
            await Assert.ThrowsAsync<DomainException>(act);

            _mocker.GetMock<IPostRepository>()
                .Verify(x => x.UpdatePostAsync(It.IsAny<Post>()), Times.Never);
        }

        [Fact]
        public async Task SubmitPostAsync_ShouldUpdatePost_WhenPostExists()
        {
            //Arrange
            var post = new Post("title", "content", 1);

            _mocker.GetMock<IPostRepository>()
                .Setup(x => x.GetPostByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(post);

            //Act
            await _sut.SubmitPostAsync(1, 1);

            //Assert
            _mocker.GetMock<IPostRepository>()
                .Verify(x => x.UpdatePostAsync(It.Is<Post>(p => p.Status == PostStatus.Pending)), Times.Once);
        }

        [Fact]
        public async Task GetPendingPostsAsync_ShouldReturnPendginPosts()
        {
            //Arrange
            var post = new Post("title", "content", 1);
            
            _mocker.GetMock<IPostRepository>()
                .Setup(x => x.GetPostsByStatusAsync(It.IsAny<PostStatus>()))
                .ReturnsAsync(new List<Post> { post });

            //Act
            var result = await _sut.GetPendingPostsAsync();

            //Assert                                    
            Assert.Single(result);
            _mocker.GetMock<IPostRepository>()
                .Verify(x => x.GetPostsByStatusAsync(It.Is<PostStatus>(p => p == PostStatus.Pending)), Times.Once);
        }

        [Fact]
        public async Task ApprovePostAsync_ShouldThrowDomainException_WhenPostDoesNotExist()
        {            
            //Act
            Task act() => _sut.ApprovePostAsync(1);

            //Assert
            await Assert.ThrowsAsync<DomainException>(act);            
            _mocker.GetMock<IPostRepository>()
                .Verify(x => x.UpdatePostAsync(It.IsAny<Post>()), Times.Never);
        }

        [Fact]
        public async Task ApprovePostAsync_ShouldUpdatePost_WhenPostExists()
        {
            //Arrange
            var post = new Post("title", "content", 1);
            post.Submit();

            _mocker.GetMock<IPostRepository>()
                .Setup(x => x.GetPostByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(post);

            //Act
            await _sut.ApprovePostAsync(1);

            //Assert            
            _mocker.GetMock<IPostRepository>()
                .Verify(x => x.UpdatePostAsync(It.Is<Post>(p => p.Status == PostStatus.Published && p.PublishedAt != null)), Times.Once);
        }

        [Fact]
        public async Task RejectPostAsync_ShouldThrowDomainException_WhenPostDoesNotExist()
        {
            //Act
            Task act() => _sut.RejectPostAsync(1, 1);

            //Assert
            await Assert.ThrowsAsync<DomainException>(act);
            _mocker.GetMock<IPostRepository>()
                .Verify(x => x.UpdatePostAsync(It.IsAny<Post>()), Times.Never);
        }

        [Fact]
        public async Task RejectPostAsync_ShouldUpdatePost_WhenPostExists()
        {
            //Arrange
            var post = new Post("title", "content", 1);
            post.Submit();

            _mocker.GetMock<IPostRepository>()
                .Setup(x => x.GetPostByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(post);

            //Act
            await _sut.RejectPostAsync(1, 1);

            //Assert            
            _mocker.GetMock<IPostRepository>()
                .Verify(x => x.UpdatePostAsync(It.Is<Post>(p => p.Status == PostStatus.Rejected)), Times.Once);
        }
    }
}
