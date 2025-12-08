using Lab6.Config;
using Lab6.Extensions;
using Lab6.Logic;
using Lab6.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Lab6.Controllers
{
    [Authorize]
    public class BlogController : Controller
    {
        private readonly IBlogRepo _repo;
        private readonly BlogConfig _config;

        public BlogController(IBlogRepo repo, IOptionsSnapshot<BlogConfig> config)
        {
            _repo = repo;
            _config = config.Value;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var posts = (await _repo.GetAllAsync())
                .OrderByDescending(p => p.TimeStamp)
                .Select(p => p.ToView())
                .ToList();

            ViewBag.DateFormat = _config.DateFormatSwitch;
            ViewBag.SummaryWordCount = _config.SummaryWordCount;

            return View(posts);
        }

        [AllowAnonymous]
        public async Task<IActionResult> Details(int id)
        {
            var post = await _repo.GetByIdAsync(id);
            if (post == null) return NotFound();

            var model = post.ToView(); // Ensure 'post' is awaited and is of type 'BlogPost'  
            ViewBag.DateFormat = _config.DateFormatSwitch;

            return View(model);
        }

        public IActionResult Create()
        {
            return View(new BlogPostModel());
        }

        [HttpPost]
        public IActionResult Create(BlogPostModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var data = model.ToData();
            data.ID = null;

            _repo.UpsertAsync(data);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var post = await _repo.GetByIdAsync(id); // Ensure 'post' is awaited and is of type 'BlogPost'  
            if (post == null) return NotFound();

            var model = post.ToView();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, BlogPostModel model)
        {
            if (id != model.ID)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var exists = await _repo.GetByIdAsync(id); // Await the Task<BlogPost> to get the BlogPost object
            if (exists == null) return NotFound();

            exists.Title = model.Title;
            exists.Content = model.Content;
            exists.Author = model.Author;

            _repo.UpsertAsync(exists);

            return RedirectToAction(nameof(Index));
        }
    }
}
