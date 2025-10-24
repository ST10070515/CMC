using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PROG6212_CMCS.Data;

//[Authorize(Roles = "HR")]
public class HRController : Controller
{
    private readonly ApplicationDbContext _context;

    public HRController(ApplicationDbContext context)
    {
        _context = context;
    }

    public IActionResult Dashboard()
    {
        return View();
    }
}