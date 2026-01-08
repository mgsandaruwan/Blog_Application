using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using BlogApplication.Models;

namespace BlogApplication.Helpers
{
    public static class BlogStorage
    {
        private static string filePath = Path.Combine(Directory.GetCurrentDirectory(), "App_Data", "posts.json");

        public static List<BlogPost> LoadPosts()
        {
            if (!File.Exists(filePath))
                return new List<BlogPost>();

            string json = File.ReadAllText(filePath);
            return JsonSerializer.Deserialize<List<BlogPost>>(json) ?? new List<BlogPost>();
        }

        public static void SavePosts(List<BlogPost> posts)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(filePath)!);
            string json = JsonSerializer.Serialize(posts, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(filePath, json);
        }
    }
}
