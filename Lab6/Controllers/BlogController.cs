using Lab6.Config;
using Lab6.Extensions;
using Lab6.Logic;
using Lab6.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Lab6.Controllers
{
    public class BlogController : Controller
    {
        private readonly IBlogRepo _repo;
        private readonly BlogConfig _config;

        public BlogController(IBlogRepo repo, IOptionsSnapshot<BlogConfig> config)
        {
            _repo = repo;
            _config = config.Value;
        }

        public IActionResult Index()
        {
            var posts = _repo.GetAll()
                .OrderByDescending(p => p.TimeStamp)
                .Select(p => p.ToView())
                .ToList();

            ViewBag.DateFormat = _config.DateFormatSwitch;
            ViewBag.SummaryWordCount = _config.SummaryWordCount;

            return View(posts);
        }

        public IActionResult Details(int id)
        {
            var post = _repo.GetById(id);
            if (post == null) return NotFound();

            var model = post.ToView();
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

            _repo.Upsert(data);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var post = _repo.GetById(id);
            if (post == null) return NotFound();

            var model = post.ToView();
            return View(model);
        }

        [HttpPost]
        public IActionResult Edit(int id, BlogPostModel model)
        {
            if (id != model.ID)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var exists = _repo.GetById(id);
            if (exists == null) return NotFound();

            exists.Title = model.Title;
            exists.Content = model.Content;
            exists.Author = model.Author;

            _repo.Upsert(exists);

            return RedirectToAction(nameof(Index));
        }
    }
}
