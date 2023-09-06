using System.ComponentModel.DataAnnotations;

namespace BlogApi.Requests
{
    public class NewPostModel
    {
        [Required]
        public string Title { get; set; }
        [Required]
        public string Content { get; set; }
    }
}
