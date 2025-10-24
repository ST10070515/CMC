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
        DashboardData data = new DashboardData();

        // Get DB values for the dashboard for this user.
        var iterClaims = _context.Claims;
        claimListModel = iterClaims.ToList<Claim>();

        data.allClaims = claimListModel.Count();
        data.pendingClaims = claimListModel.Where(c => c.ClaimStatus == ClaimStatus.PENDING).Count();
        data.approvedClaims = claimListModel.Where(c => c.ClaimStatus == ClaimStatus.APPROVED).Count();
        data.rejectedClaims = claimListModel.Where(c => c.ClaimStatus == ClaimStatus.REJECTED).Count();
        data.claims = claimListModel;

        // Populate the view model, and pass to teh view. 
        return View(data);
    }
}

