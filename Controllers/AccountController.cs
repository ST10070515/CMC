using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using Newtonsoft.Json.Linq;
using PROG6212_CMCS.Data;
using PROG6212_CMCS.Models;
using PROG6212_CMCS.Models.ViewModels;
using System;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Claim = System.Security.Claims.Claim;

namespace PROG6212_CMCS
{
    public class AccountController : Controller
    {

        private ApplicationDbContext _context;

        public AccountController(ApplicationDbContext context) {
            _context = context;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new User
                {
                    Email = model.Email,
                    FirstName = model.FirstName,   
                    LastName = model.LastName,   
                    CellNumber = model.CellNumber,
                    Password = model.Password,     
                    Role = model.Role,
                    IsActive = true,               
                    DateCreated = DateTime.Now    
                };

                _context.MyUsers.Add(user);
                _context.SaveChanges();
            }

            return RedirectToAction("login", "Account");
        }


        [HttpGet]
        public IActionResult Login(string returnUrl = "")
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(LoginViewModel model, string returnUrl = "")
        {
            if (ModelState.IsValid)
            {
                User user;
                var loginSuccessful = false;
                var quarableUser = _context.MyUsers.Where(u => u.Email == model.Email); 
                
                if (quarableUser.Any() == false) {

                    ModelState.AddModelError(string.Empty, "Login failed, user record missing.");
                    return View(model);
                }
                else
                {
                    user = quarableUser.First();
                    if ((user.Password == model.Password) && (user.Email == model.Email))
                    {
                        loginSuccessful = true;
                    }
                }

                if (loginSuccessful)
                {
                    if (returnUrl == "")
                    {
                        if (user.Role == RoleName.Lecturer)
                        {
                            return RedirectToAction("Dashboard", "Lecturer", new { userId = user.UserID});
                        }
                        else if (user.Role == RoleName.ProgramCoordinator)
                        {
                            return RedirectToAction("Dashboard", "ProgrammeCoordinator", new { userId = user.UserID });
                        }
                        else if (user.Role == RoleName.Manager)
                        {
                            return RedirectToAction("Dashboard", "AcademicManager", new { userId = user.UserID });
                        }
                        else if (user.Role == RoleName.HR)
                        {
                            return RedirectToAction("Dashboard", "HR", new { userId = user.UserID });
                        }

                    }
                    else
                    {
                        return LocalRedirect(returnUrl ?? "~/");
                    }
                }
                else
                {   
                    ModelState.AddModelError(string.Empty, "Login failed, wrong password.");
                }
            }

            return View(model);
        }
      
    }
}
