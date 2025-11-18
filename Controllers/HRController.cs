using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PROG6212_CMCS.Data;
using PROG6212_CMCS.Models;
using PROG6212_CMCS.Models.ViewModels;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using System.IO;



namespace PROG6212_CMCS.Controllers
{
    public class HRController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HRController(ApplicationDbContext context)
        {
            _context = context;
        }
        
        [HttpGet]
        public IActionResult Dashboard([FromQuery] int userId)
        {
            ViewBag.userId = userId;

            DashboardData data = new DashboardData();
            List<ClaimItem> claimItems = new List<ClaimItem>();

            var claims = _context.Claims
                .Include(c => c.User) 
                .ToList();

            if (claims.Any())
            {
                data.totalApprovedClaims = claims.Count(c => c.ClaimStatus == ClaimStatus.APPROVED);
                data.totalAmountPayable = claims
                    .Where(c => c.ClaimStatus == ClaimStatus.APPROVED)
                    .Sum(c => c.ClaimAmount); 
                data.totalLecturers = _context.MyUsers.Count(u => u.Role == RoleName.Lecturer);

                foreach (var claim in claims.Where(c => c.ClaimStatus == ClaimStatus.APPROVED))
                {
                    var user = _context.MyUsers.FirstOrDefault(u => u.UserID == claim.UserID);
                    var docs = _context.Documents.Where(d => d.ClaimID == claim.ClaimID)
                        .ToList();

                    claimItems.Add(new ClaimItem(claim, docs, user!));
                }

                data.claims = claimItems;
            }

            return View(data);
        }

        [HttpGet]
        public IActionResult GenerateInvoice(int claimId)
        {
            var claim = _context.Claims.FirstOrDefault(c => c.ClaimID == claimId);
            var lecturer = _context.MyUsers.FirstOrDefault(u => u.UserID == claim!.UserID);

            if (claim == null || lecturer == null)
                return NotFound("Claim or lecturer not found.");

            using (var stream = new MemoryStream())
            {
                PdfWriter writer = new PdfWriter(stream);
                PdfDocument pdf = new PdfDocument(writer);
                iText.Layout.Document document = new iText.Layout.Document(pdf);

                document.Add(new Paragraph($"Invoice for Claim #{claim.ClaimID}"));
                document.Add(new Paragraph($"Lecturer: {lecturer.FirstName} {lecturer.LastName}"));
                document.Add(new Paragraph($"Course: {claim.CourseName}"));
                document.Add(new Paragraph($"Claim Amount: R {claim.ClaimAmount:N2}"));
                document.Add(new Paragraph($"Date: {DateTime.Now:yyyy-MM-dd}"));

                document.Close();

                return File(stream.ToArray(), "application/pdf", $"Invoice_Claim_{claim.ClaimID}.pdf");
            }
        }


    }
}
