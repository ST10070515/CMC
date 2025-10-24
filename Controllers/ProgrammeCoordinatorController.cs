// Inside PROG6212_CMCS.Controllers/ProgrammeCoordinatorController.cs

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PROG6212_CMCS.Data;
using PROG6212_CMCS.Models;
using PROG6212_CMCS.Models.ViewModels; // <-- Don't forget this using

//[Authorize(Roles = "ProgrammeCoordinator")]
public class ProgrammeCoordinatorController : Controller
{
    private readonly ApplicationDbContext _context;

    public ProgrammeCoordinatorController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    public IActionResult ApproveClaim([FromQuery] int claimId, [FromQuery] int userId)
    {
        // Store the query param
        ViewBag.userId = userId;

        // Update the claim
        var iterClaims = _context.Claims.Where(c => c.ClaimID == claimId);
        if (iterClaims.Any())
        {
            iterClaims.First().ClaimStatus = ClaimStatus.APPROVED;
            _context.SaveChanges();
        }

        // Populate the view model, and pass to the view. 
        return RedirectToAction("Dashboard", "AcademicManager", new { userId });
    }

    [HttpPost]
    public IActionResult RejectClaim([FromQuery] int claimId, [FromQuery] int userId)
    {
        // Store the query param
        ViewBag.userId = userId;

        // Update the claim
        var iterClaims = _context.Claims.Where(c => c.ClaimID == claimId);
        if (iterClaims.Any())
        {
            iterClaims.First().ClaimStatus = ClaimStatus.REJECTED;
            _context.SaveChanges();
        }

        // Populate the view model, and pass to the view. 
        return RedirectToAction("Dashboard", "AcademicManager", new { userId });
    }

    [HttpGet]
    public IActionResult Dashboard([FromQuery] int userId)
    {
        // Store the query param
        ViewBag.userId = userId;

        // Process the request
        List<Claim> claimListModel;
        List<ClaimItem> claimItem = new List<ClaimItem>();
        DashboardData data = new DashboardData();

        // Get DB values for the dashboard for this user.
        var iterClaims = _context.Claims;

        if (iterClaims.Any())
        {
            claimListModel = iterClaims.ToList<Claim>();
            data.allClaims = claimListModel.Count();
            data.pendingClaims = claimListModel.Where(c => c.ClaimStatus == ClaimStatus.PENDING).Count();
            data.approvedClaims = claimListModel.Where(c => c.ClaimStatus == ClaimStatus.APPROVED).Count();

            // Add all claims and data in view
            foreach (var claim in claimListModel)
            {
                // Get all documents for the claim
                List<Document> documents = _context.Documents.Where(c => c.ClaimID == claim.ClaimID).ToList();

                // Get the user linked to the claim
                User user = _context.MyUsers.Where(u => u.UserID == claim.UserID).First();

                // Create the structure
                claimItem.Add(new ClaimItem(claim, documents, user));
            }

            data.claims = claimItem;
        }

        // Populate the view model, and pass to teh view. 
        return View(data);
    }
}

