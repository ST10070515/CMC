using Microsoft.AspNetCore.Mvc;
using PROG6212_CMCS.Data; 
using PROG6212_CMCS.Models; 
using PROG6212_CMCS.Models.ViewModels;

namespace PROG6212_CMCS.Controllers
{
    public class CommonController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CommonController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> DownloadDocument([FromQuery] int docId)
        {
            var document = await _context.Documents.FindAsync(docId);

            if (document == null)
                return NotFound();

            return File(document.FileData, document.ContentType, document.FileName);
        }
    }
}
