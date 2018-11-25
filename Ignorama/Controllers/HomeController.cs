﻿using Ignorama.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Ignorama.Controllers
{
    public class HomeController : Controller
    {
        private readonly ForumContext _context;
        private readonly IHostingEnvironment _hostingEnvironment;

        public HomeController(
            ForumContext context,
            IHostingEnvironment hostingEnvironment)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
        }

        public IActionResult Index()
        {
            return View(_context.Tags.OrderBy(tag => tag.Name));
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

        public class ImageUploadModel
        {
            public string filename { get; set; }
            public IFormFile file { get; set; }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UploadFile(ImageUploadModel upload)
        {
            var allowedTypes = new[] {
                "image/gif", "image/jpeg", "image/jpg", "image/pjpeg", "image/x-png", "image/png", "video/webm"
            };
            var allowedExts = new[]
            {
                ".gif", ".jpeg", ".jpg", ".png", ".webm"
            };
            var ext = Path.GetExtension(upload.filename);
            if (upload.file.Length > 0 &&
                allowedTypes.Contains(upload.file.ContentType.ToLower()) &&
                allowedExts.Contains(ext.ToLower()))
            {
                using (var stream = new FileStream(
                    Path.Combine(_hostingEnvironment.WebRootPath, "uploads/", upload.filename), FileMode.Create))
                {
                    await upload.file.CopyToAsync(stream);

                    return new ContentResult
                    {
                        Content = "<script>error = 'none';</script>",
                        ContentType = "text/html",
                    };
                }
            }

            return new ContentResult
            {
                Content = "<script>error = 'Could not upload file: Unsupported file type.';</script>",
                ContentType = "text/html",
            };
        }
    }
}