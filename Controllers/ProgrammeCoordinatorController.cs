// Inside PROG6212_CMCS.Controllers/ProgrammeCoordinatorController.cs

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PROG6212_CMCS.Data;
using PROG6212_CMCS.Models.ViewModels; // <-- Don't forget this using

//[Authorize(Roles = "ProgrammeCoordinator")]
public class ProgrammeCoordinatorController : Controller
{
    private readonly ApplicationDbContext _context;

    public ProgrammeCoordinatorController(ApplicationDbContext context)
    {
        _context = context;
    }
    [HttpGet]
    public IActionResult Dashboard()
    {
        int submittedId = 1;

        return View();
    }
}

