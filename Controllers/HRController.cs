using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PROG6212_CMCS.Data;
using PROG6212_CMCS.Models;
using PROG6212_CMCS.Models.ViewModels;
using System.IO;
using static Org.BouncyCastle.Utilities.Test.FixedSecureRandom;



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

        [HttpGet]
        public async Task<IActionResult> ManageLecturers()
        {
            var lecturers = await _context.MyUsers
                                          .Where(u => u.Role == RoleName.Lecturer)
                                          .OrderBy(u => u.LastName)
                                          .ToListAsync();
            return View(lecturers);
        }


        [HttpGet]
        public IActionResult CreateLecturer()
        {
            return View(new LecturerManagementViewModel());
        }

    
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateLecturer(LecturerManagementViewModel model)
        {
            if (ModelState.IsValid)
            {
                var newUser = new User 
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email,
                    CellNumber = model.CellNumber, 
                    Role = RoleName.Lecturer 
                };

                _context.MyUsers.Add(newUser);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = $"Lecturer {newUser.LastName} added successfully.";
                return RedirectToAction(nameof(ManageLecturers));
            }

            return View(model);
        }


        [HttpGet]
        public async Task<IActionResult> EditLecturer(int userId)
        {
            var lecturer = await _context.MyUsers.FindAsync(userId);

            if (lecturer == null || lecturer.Role != RoleName.Lecturer)
                return NotFound();

            var model = new LecturerManagementViewModel
            {
                UserID = lecturer.UserID,
                FirstName = lecturer.FirstName,
                LastName = lecturer.LastName,
                Email = lecturer.Email,
                CellNumber = lecturer.CellNumber 
            };

            return View(model); 
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditLecturer(LecturerManagementViewModel model)
        {
            if (ModelState.IsValid)
            {
                var lecturerToUpdate = await _context.MyUsers.FindAsync(model.UserID);

                if (lecturerToUpdate == null || lecturerToUpdate.Role != RoleName.Lecturer)
                {
                    ModelState.AddModelError("", "Lecturer not found or unauthorized.");
                    return View(model);
                }

                lecturerToUpdate.FirstName = model.FirstName;
                lecturerToUpdate.LastName = model.LastName;
                lecturerToUpdate.Email = model.Email;
                lecturerToUpdate.CellNumber = model.CellNumber; 

                _context.MyUsers.Update(lecturerToUpdate);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = $"Lecturer {model.LastName} updated successfully.";
                return RedirectToAction(nameof(ManageLecturers));
            }
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteLecturer(int userId)
        {
            var lecturer = await _context.MyUsers.FindAsync(userId);

            if (lecturer == null || lecturer.Role != RoleName.Lecturer)
            {
                TempData["ErrorMessage"] = "Lecturer not found or cannot be deleted.";
                return RedirectToAction(nameof(ManageLecturers));
            }


            var claims = _context.Claims.Where(c => c.UserID == userId).ToList();

            if (claims.Any())
            {
                var claimIds = claims.Select(c => c.ClaimID).ToList();
                var documents = _context.Documents.Where(d => claimIds.Contains(d.ClaimID)).ToList();

                if (documents.Any())
                {
                    _context.Documents.RemoveRange(documents);
                }

                _context.Claims.RemoveRange(claims);
            }

            _context.MyUsers.Remove(lecturer);

            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = $"Lecturer {lecturer.LastName} and all related claims/documents have been deleted.";
            return RedirectToAction(nameof(ManageLecturers));
        }
    }


}