namespace PROG6212_CMCS.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using PROG6212_CMCS.Data;  // For ApplicationDbContext
    using PROG6212_CMCS.Models;  // For Claim and other models
    using PROG6212_CMCS.Models.ViewModels; // Required for SubmitClaimViewModel


    //[Authorize(Roles = "Lecturer")]
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
