using Microsoft.AspNetCore.Mvc;
using PROG6212_CMCS.Data; 
using PROG6212_CMCS.Models;  
using PROG6212_CMCS.Models.ViewModels; 


public class LecturerController : Controller
{
    private readonly ApplicationDbContext _context;

    public LecturerController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }

    [HttpGet]
    public IActionResult SubmitClaim([FromQuery] int userId)
    {
        ViewBag.userId = userId;

        return View(new SubmitClaimViewModel());
    }

    [HttpPost]
    public IActionResult SubmitClaim(SubmitClaimViewModel model, [FromQuery] int userId, List<IFormFile> ClaimFiles)
    {
        ViewBag.userId = userId;

        foreach (var file in ClaimFiles) {
            if (file.Length > 634880) {
                model.ErrorMessage = "File size exceeds 5MB";
                return View(model);
            }
        }

        var claim = new Claim
        {
            UserID = userId,
            CourseName = model.CourseName,
            HoursWorked = model.HoursWorked,
            HourlyRate = model.HourlyRate,
            ClaimAmount = model.HoursWorked * model.HourlyRate,
            AdditionalNotes = model.AdditionalNotes
        };

        _context.Claims.Add(claim);
        _context.SaveChanges();

        int newClaimId = _context.Claims.Where(u => u.UserID == userId).OrderBy(u=>u.ClaimID).Last().ClaimID;

        foreach (var ClaimFile in ClaimFiles) {
            if (ClaimFile != null && ClaimFile.Length > 0)
            {
                using (var memoryStream = new MemoryStream())
                {
                    ClaimFile.CopyTo(memoryStream);

                    var document = new Document
                    {
                        FileName = ClaimFile.FileName,
                        ContentType = ClaimFile.ContentType,
                        FileData = memoryStream.ToArray(),
                        ClaimID = newClaimId
                    };

                    _context.Documents.Add(document);
                    _context.SaveChanges();
                }
            }
        }

        return RedirectToAction("Dashboard", "Lecturer", new { userId});
    }

    [HttpGet]
    public IActionResult Dashboard([FromQuery] int userId)
    {
        ViewBag.userId = userId;
         
        List<Claim> claimListModel;
        List<ClaimItem> claimItem = new List<ClaimItem>();
        DashboardData data = new DashboardData();

        var iterClaims = _context.Claims.Where(c => c.UserID == userId);

        if (iterClaims.Any())
        {
            claimListModel = iterClaims.ToList<Claim>();
            data.allClaims = claimListModel.Count();
            data.pendingClaims = claimListModel.Where(c => c.ClaimStatus == ClaimStatus.PENDING).Count();
            data.approvedClaims = claimListModel.Where(c => c.ClaimStatus == ClaimStatus.APPROVED).Count();

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

    [HttpGet]
    public IActionResult MyClaims()
    { 
        return View();
    }

}

