using Lab6.Config;
using Lab6.Logic;
using Lab6.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Security.Claims;

namespace Lab6.Controllers
{
    public class BlogController : Controller
    {
        private readonly IBlogRepo _repo;
        private readonly BlogConfig _config;

        public BlogController(IBlogRepo repo, IOptions<BlogConfig> blogOptions)
        {
            _repo = repo;
            _config = blogOptions.Value;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var posts = await _repo.GetAllAsync();

            ViewBag.DateFormat = _config.DateFormatSwitch;
            ViewBag.SummaryWordCount = _config.SummaryWordCount;

            return View(posts);
        }

        [AllowAnonymous]
        public async Task<IActionResult> Details(int id)
        {
            var post = await _repo.GetByIdAsync(id);
            if (post == null) return NotFound();

            ViewBag.DateFormat = _config.DateFormatSwitch;

            var model = new BlogPostModel
            {
                ID = post.ID,
                Title = post.Title,
                Content = post.Content,
                Author = post.Author,
                Timestamp = post.TimeStamp
            };

            return View(model);
        }

        [Authorize]
        public IActionResult Create()
        {
            return View(new BlogPostModel());
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create(BlogPostModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var post = new BlogPost
            {
                Title = model.Title,
                Content = model.Content,
                Author = User.Identity?.Name ?? "Unknown",
                TimeStamp = DateTime.Now
            };

            await _repo.UpsertAsync(post);
            return RedirectToAction(nameof(Index));
        }

        [Authorize]
        public async Task<IActionResult> Edit(int id)
        {
            var post = await _repo.GetByIdAsync(id);
            if (post == null) return NotFound();

            var model = new BlogPostModel
            {
                ID = post.ID,
                Title = post.Title,
                Content = post.Content,
                Author = post.Author,
                Timestamp = post.TimeStamp
            };

            return View(model);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Edit(int id, BlogPostModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var post = new BlogPost
            {
                ID = id,
                Title = model.Title,
                Content = model.Content,
                Author = User.Identity?.Name ?? model.Author,
                TimeStamp = DateTime.Now
            };

            await _repo.UpsertAsync(post);
            return RedirectToAction(nameof(Index));
        }
    }
}