using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using socail_web2.Models;
using System.Diagnostics;
using socail_web2.Data;
using Microsoft.EntityFrameworkCore;
using socail_web2.ViewModels;

namespace socail_web2.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> IndexAsync()
        {
            var posts = await (from p in _context.Post
                              join u in _context.Users on p.ApplicationUserId equals u.Id
                              select new PostViewModel
                              {
                                  Id = p.Id,
                                  Title = p.Title,
                                  Description = p.Description,
                                  ImagePath = p.Image,
                                  UserId = p.ApplicationUserId,
                                  UseName = u.UserName
                              }
                ).ToListAsync();

            var user = await (_context.Users).ToListAsync();

            var indexView = new IndexViewModel();
            indexView.Users = user;
            indexView.Posts = posts;
            return View(indexView);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}