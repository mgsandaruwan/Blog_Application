using Microsoft.AspNetCore.Http;
using System.Text.Json.Serialization;

namespace BlogApplication.Models
{
    public class BlogPost
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Category { get; set; }
        public string ImageUrl { get; set; }
        public string Content { get; set; }

        [JsonIgnore] // ✅ this line is key
        public IFormFile ImageFile { get; set; }
    }
}
