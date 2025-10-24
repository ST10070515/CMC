using Microsoft.AspNetCore.Mvc;
using PROG6212_CMCS.Data;  // For ApplicationDbContext
using PROG6212_CMCS.Models;  // For Claim and other models
using PROG6212_CMCS.Models.ViewModels; // Required for SubmitClaimViewModel


//[Authorize(Roles = "Lecturer")]
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
        // Store the query param
        ViewBag.userId = userId;

        // Open the view
        return View(new SubmitClaimViewModel());
    }

    [HttpPost]
    public IActionResult SubmitClaim(SubmitClaimViewModel model, [FromQuery] int userId, List<IFormFile> ClaimFiles)
    {
        // Store the query param
        ViewBag.userId = userId;

        // Validate file size
        foreach (var file in ClaimFiles) {
            if (file.Length > 634880) {
                model.ErrorMessage = "File size exceeds 5MB";
                return View(model);
            }
        }

        // Process request 
        var claim = new Claim
        {
            UserID = userId,
            CourseName = model.CourseName,
            HoursWorked = model.HoursWorked,
            HourlyRate = model.HourlyRate,
            ClaimAmount = model.HoursWorked * model.HourlyRate,
            AdditionalNotes = model.AdditionalNotes
        };

        // Insert data in the db
        _context.Claims.Add(claim);
        _context.SaveChanges();

        // Get the claim ID
        int newClaimId = _context.Claims.Where(u => u.UserID == userId).OrderBy(u=>u.ClaimID).Last().ClaimID;

        // Process documents
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
        // Store the query param
        ViewBag.userId = userId;
         
        // Process the request
        List<Claim> claimListModel;
        List<ClaimItem> claimItem = new List<ClaimItem>();
        DashboardData data = new DashboardData();

        // Get DB values for the dashboard for this user.
        var iterClaims = _context.Claims.Where(c => c.UserID == userId);

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

    [HttpGet]
    public IActionResult MyClaims()
    { 
        return View();
    }

    /*public class SubmitClaimViewModel
    {
        // ... Existing form properties ...

        // --- PROPERTIES ADDED TO FIX CS1061 ERRORS ON DASHBOARD ---
        // These properties are required because the Dashboard view expects them in the model it receives.
        public int ClaimID { get; set; }
        public decimal Amount { get; set; }
        public DateTime SubmissionDate { get; set; }
        public List<SelectListItem> AvailableCourses { get; internal set; }


        // Ensure 'ClaimStatuses' matches the name of your enum in the Models folder!
        // --- END FIX ---
    }*/
}

