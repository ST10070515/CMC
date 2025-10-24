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
        return View();
    }

    [HttpPost]
    public IActionResult SubmitClaim(Claim model, [FromQuery] int userId)
    {
        // Store the query param
        ViewBag.userId = userId;

        // Process request 
        var claim = new Claim
        {
            UserID = userId,
            CourseName=model.CourseName,
            HoursWorked = model.HoursWorked,
            HourlyRate = model.HourlyRate,
            ClaimAmount = model.HoursWorked * model.HourlyRate,
            AdditionalNotes = model.AdditionalNotes
        };

        // Insert data in the db
        _context.Claims.Add(claim);
        _context.SaveChanges();

        // Add a success message

        return View(model);
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
        var iterClaims = _context.Claims.Where(c => c.UserID == userId);

        if (iterClaims.Any())
        {
            claimListModel = iterClaims.ToList<Claim>();

            data.allClaims = claimListModel.Count();
            data.pendingClaims = claimListModel.Where(c => c.ClaimStatus == ClaimStatus.PENDING).Count();
            data.approvedClaims = claimListModel.Where(c => c.ClaimStatus == ClaimStatus.APPROVED).Count();
            data.claims = claimListModel;
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

