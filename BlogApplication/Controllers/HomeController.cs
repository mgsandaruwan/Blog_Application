using Microsoft.AspNetCore.Mvc;
using BlogApplication.Models;
using BlogApplication.Helpers;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Hosting;

namespace BlogApplication.Controllers
{
    public class HomeController : Controller
    {
        private readonly IWebHostEnvironment _environment;
        private static List<BlogPost> posts;

        public HomeController(IWebHostEnvironment environment)
        {
            _environment = environment;
            posts ??= BlogStorage.LoadPosts(); // Load posts from JSON when app starts
        }

        // -------------------------
        // HOME PAGE
        // -------------------------
        public IActionResult Index()
        {
            // Group posts by category for home page display
            var grouped = posts
                .GroupBy(p => p.Category)
                .ToDictionary(g => g.Key, g => g.ToList());

            return View(grouped);
        }

        // -------------------------
        // ADD POST (GET)
        // -------------------------
        [HttpGet]
        public IActionResult AddPost()
        {
            return View();
        }

        // -------------------------
        // ADD POST (POST)
        // -------------------------
        [HttpPost]
        public IActionResult AddPost(BlogPost model)
        {
            if (model.ImageFile != null && model.ImageFile.Length > 0)
            {
                string uploadsFolder = Path.Combine(_environment.WebRootPath, "images");
                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                string uniqueFileName = Path.GetFileName(model.ImageFile.FileName);
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    model.ImageFile.CopyTo(stream);
                }

                model.ImageUrl = "/images/" + uniqueFileName;
            }

            model.Id = posts.Count > 0 ? posts.Max(p => p.Id) + 1 : 1;
            posts.Add(model);

            // Save all posts permanently to JSON
            BlogStorage.SavePosts(posts);

            return RedirectToAction("Index");
        }

        // -------------------------
        // READ FULL POST
        // -------------------------
        public IActionResult ReadPost(int id)
        {
            var post = posts.FirstOrDefault(p => p.Id == id);
            if (post == null)
                return NotFound();

            return View(post);
        }

        // -------------------------
        // CATEGORY PAGES
        // -------------------------
        public IActionResult Travel()
        {
            var travelPosts = posts.Where(p => p.Category == "Travel").ToList();
            return View("Category", travelPosts);
        }

        public IActionResult Food()
        {
            var foodPosts = posts.Where(p => p.Category == "Agriculture").ToList();
            return View("Category", foodPosts);
        }

        public IActionResult Sports()
        {
            var sportsPosts = posts.Where(p => p.Category == "Sports").ToList();
            return View("Category", sportsPosts);
        }

        // -------------------------
        // ABOUT PAGE
        // -------------------------
        public IActionResult About()
        {
            return View();
        }
    }
}
