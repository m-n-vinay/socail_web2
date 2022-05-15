using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using socail_web2.Data;
using socail_web2.Models;
using socail_web2.ViewModels;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace socail_web2.Controllers
{
    [Authorize]
    public class PostsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public PostsController(ApplicationDbContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _webHostEnvironment = hostEnvironment;
        }

        // GET: Posts
        public async Task<IActionResult> Index()
        {
            return View(await _context.Post.ToListAsync());
        }

        // GET: Posts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await _context.Post
                .FirstOrDefaultAsync(m => m.Id == id);
            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }

        // GET: Posts/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Posts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,Description,Image")] PostViewModel postViewModel)
        {
            if (ModelState.IsValid)
            {
                string uniqueFileName = ProcessUploadedFile(postViewModel);

                if(uniqueFileName != null)
                {
                    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                   

                    var post = new Post();

                    post.Title = postViewModel.Title;
                    post.Description = postViewModel.Description;
                    post.Image = uniqueFileName;
                    post.ApplicationUserId = userId;

                    _context.Add(post);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index), "Home");
                }
                ModelState.AddModelError(nameof(PostViewModel.Image), "Upload a valid image.");
            }
            return View(postViewModel);
        }

        // GET: Posts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await _context.Post.FindAsync(id);
            if (post == null)
            {
                return NotFound();
            }
            return View(post);
        }

        // POST: Posts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,ApplicationUserId,Image")] Post post)
        {
            if (id != post.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(post);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PostExists(post.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(post);
        }

        // GET: Posts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await _context.Post
                .FirstOrDefaultAsync(m => m.Id == id);
            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }

        // POST: Posts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var post = await _context.Post.FindAsync(id);
            _context.Post.Remove(post);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PostExists(int id)
        {
            return _context.Post.Any(e => e.Id == id);
        }

        private bool IsImage(PostViewModel model)
        {
            if (model.Image != null && 
                    model.Image.ContentType.ToLower() == "image/jpg" ||
                    model.Image.ContentType.ToLower() == "image/jpeg" ||
                    model.Image.ContentType.ToLower() == "image/pjpeg" ||
                    model.Image.ContentType.ToLower() == "image/gif" ||
                    model.Image.ContentType.ToLower() == "image/x-png" ||
                    model.Image.ContentType.ToLower() == "image/png")
            {
                return true;
            }
            return false;
        }

        private string ProcessUploadedFile(PostViewModel model)
        {
            var isImage = IsImage(model);
            if (isImage)
            {
                string uniqueFileName = null;
                string folderName = null;
                if (model.Image != null)
                {
                    folderName = "ImagesOfPost";
                    string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, folderName);
                    Directory.CreateDirectory(uploadsFolder);
                    uniqueFileName = Guid.NewGuid().ToString() + "_" + model.Image.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        model.Image.CopyTo(fileStream);
                    }
                    if (uniqueFileName == null || folderName == null)
                    {
                        return null;
                    }
                    return $"{folderName}/{uniqueFileName}";
                }
            }
            return null;
            
        }
    }
}
