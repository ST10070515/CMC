using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PROG6212_CMCS.Data;
using PROG6212_CMCS.Models;
using PROG6212_CMCS.Models.ViewModels; 

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
        ViewBag.userId = userId;

        var iterClaims = _context.Claims.Where(c => c.ClaimID == claimId);
        if (iterClaims.Any())
        {
            iterClaims.First().ClaimStatus = ClaimStatus.APPROVED;
            _context.SaveChanges();
        }

        return RedirectToAction("Dashboard", "AcademicManager", new { userId });
    }

    [HttpPost]
    public IActionResult RejectClaim([FromQuery] int claimId, [FromQuery] int userId)
    {
        ViewBag.userId = userId;

        var iterClaims = _context.Claims.Where(c => c.ClaimID == claimId);
        if (iterClaims.Any())
        {
            iterClaims.First().ClaimStatus = ClaimStatus.REJECTED;
            _context.SaveChanges();
        }

        return RedirectToAction("Dashboard", "AcademicManager", new { userId });
    }

    [HttpGet]
    public IActionResult Dashboard([FromQuery] int userId)
    {
        ViewBag.userId = userId;

        List<Claim> claimListModel;
        List<ClaimItem> claimItem = new List<ClaimItem>();
        DashboardData data = new DashboardData();

        var iterClaims = _context.Claims;

        if (iterClaims.Any())
        {
            claimListModel = iterClaims.ToList<Claim>();
            data.allClaims = claimListModel.Count();
            data.pendingClaims = claimListModel.Where(c => c.ClaimStatus == ClaimStatus.PENDING).Count();
            data.approvedClaims = claimListModel.Where(c => c.ClaimStatus == ClaimStatus.APPROVED).Count();
            data.rejectedClaims = claimListModel.Where(c => c.ClaimStatus == ClaimStatus.REJECTED).Count();

            foreach (var claim in claimListModel)
            {
                List<Document> documents = _context.Documents.Where(c => c.ClaimID == claim.ClaimID).ToList();

                User user = _context.MyUsers.Where(u => u.UserID == claim.UserID).First();

                claimItem.Add(new ClaimItem(claim, documents, user));
            }

            data.claims = claimItem;
        }

        return View(data);
    }
}

