using BlogApi.Domain.Entities;
using BlogApi.Domain.Enums;
using BlogApi.Domain.Exceptions;

namespace BlogApi.Tests.UnitTests.Domain.Entities
{
    public class PostTest
    {
        [Fact]
        public void Constructor_ShouldThrowDomainException_WhenTitleIsNull()
        {            
            //Act
            Action act = () => new Post(null, "content", 1);

            //Assert
            Assert.Throws<DomainException>(act);
        }

        [Fact]
        public void Constructor_ShouldThrowDomainException_WhenContentIsNull()
        {
            //Act
            Action act = () => new Post("title", null, 1);

            //Assert
            Assert.Throws<DomainException>(act);
        }

        [Fact]
        public void Constructor_ShouldCreateInstance_WhenParametersAreValid()
        {
            //Act
            var post = new Post("title", "content", 1);

            //Assert
            Assert.Equal("title", post.Title);
            Assert.Equal("content", post.Content);
            Assert.Equal(1, post.AuthorId);
            Assert.Equal(PostStatus.Created, post.Status);
            Assert.Empty(post.Comments);
            Assert.Null(post.PublishedAt);
        }

        [Fact]
        public void Edit_ShouldThrowDomainException_WhenTitleIsNull()
        {
            //Arrange
            var post = new Post("title", "content", 1);

            //Act
            Action act = () => post.Edit(null, "new content");

            //Assert
            Assert.Throws<DomainException>(act);
        }

        [Fact]
        public void Edit_ShouldThrowDomainException_WhenContentIsNull()
        {
            //Arrange
            var post = new Post("title", "content", 1);

            //Act
            Action act = () => post.Edit("new title", null);

            //Assert
            Assert.Throws<DomainException>(act);
        }

        [Fact]
        public void Edit_ShouldThrowDomainException_WhenPostIsAlreadyPublished()
        {
            //Arrange
            var post = new Post("title", "content", 1);

            post.Submit();
            post.Approve();

            //Act
            Action act = () => post.Edit("new title", "new content");

            //Assert
            Assert.Throws<DomainException>(act);
        }

        [Fact]
        public void Edit_ShouldThrowDomainException_WhenPostIsPending()
        {
            //Arrange
            var post = new Post("title", "content", 1);

            post.Submit();

            //Act
            Action act = () => post.Edit("new title", "new content");

            //Assert
            Assert.Throws<DomainException>(act);
        }

        [Fact]
        public void Edit_ShouldUpdateTitleAndContent_WhenStatusIsValid()
        {
            //Arrange
            var post = new Post("title", "content", 1);

            //Act
            post.Edit("new title", "new content");

            //Assert
            Assert.Equal("new title", post.Title);
            Assert.Equal("new content", post.Content);
        }

        [Fact]
        public void AddPublicComment_ShouldThrowDomainException_WhenPostIsNotPublished()
        {
            //Arrange
            var post = new Post("title", "content", 1);

            //Act
            Action act = () => post.AddPublicComment("comment", 1);

            //Assert
            Assert.Throws<DomainException>(act);
        }        

        [Fact]
        public void AddPublicComment_ShouldAddComment_WhenPostIsPublished()
        {
            //Arrange
            var post = new Post("title", "content", 1);

            post.Submit();
            post.Approve();

            //Act
            post.AddPublicComment("comment", 1);

            //Assert
            Assert.Single(post.Comments);
            Assert.Equal("comment", post.Comments.First().Content);
            Assert.False(post.Comments.First().IsRejection);
        }

        [Fact]
        public void Submit_ShouldThrowDomainException_WhenPostIsAlreadyPublished()
        {
            //Arrange
            var post = new Post("title", "content", 1);

            post.Submit();
            post.Approve();

            //Act
            Action act = () => post.Submit();

            //Assert
            Assert.Throws<DomainException>(act);
        }

        [Fact]
        public void Submit_ShouldUpdateStatus_WhenPostIsNotPublished()
        {
            //Arrange
            var post = new Post("title", "content", 1);

            //Act
            post.Submit();

            //Assert
            Assert.Equal(PostStatus.Pending, post.Status);
        }

        [Fact]
        public void Approve_ShouldThrowDomainException_WhenPostIsNotPending()
        {
            //Arrange
            var post = new Post("title", "content", 1);

            //Act
            Action act = () => post.Approve();

            //Assert
            Assert.Throws<DomainException>(act);
        }

        [Fact]
        public void Approve_ShouldUpdateStatusAndDate_WhenPostIsPending()
        {
            //Arrange
            var post = new Post("title", "content", 1);
            post.Submit();

            //Act
            post.Approve();

            //Assert
            Assert.Equal(PostStatus.Published, post.Status);
            Assert.NotNull(post.PublishedAt);
        }

        [Fact]
        public void Reject_ShouldThrowDomainException_WhenPostIsNotPending()
        {
            //Arrange
            var post = new Post("title", "content", 1);
            
            //Act
            Action act = () => post.Reject(1);

            //Assert
            Assert.Throws<DomainException>(act);
        }

        [Fact]
        public void Reject_ShouldUpdateStatus_WhenPostIsPublished()
        {
            //Arrange
            var post = new Post("title", "content", 1);
            post.Submit();

            //Act
            post.Reject(1);

            //Assert
            Assert.Equal(PostStatus.Rejected, post.Status);
            Assert.Empty(post.Comments);
        }

        [Fact]
        public void Reject_ShouldAddComment_WhenCommentIsNotNull()
        {
            //Arrange
            var post = new Post("title", "content", 1);
            post.Submit();

            //Act
            post.Reject(1, "rejection comment");

            //Assert
            Assert.Equal(PostStatus.Rejected, post.Status);
            Assert.Equal("rejection comment", post.Comments.First().Content);
            Assert.True(post.Comments.First().IsRejection);
        }
    }
}
