using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CollabTextEditor.Models;
using CollabTextEditor.Data;

namespace CollabTextEditor.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext _context;

        public HomeController(ILogger<HomeController> logger, AppDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            // Try to load the document from the database.
            var document = await _context.Documents.FirstOrDefaultAsync();
            if (document == null)
            {
                // If not found, create a new document.
                document = new Document { Content = "" };
                _context.Documents.Add(document);
                await _context.SaveChangesAsync();
            }
            // Pass the document content to the view via ViewBag.
            ViewBag.DocumentContent = document.Content;
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel 
            { 
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier 
            });
        }
    }
}
