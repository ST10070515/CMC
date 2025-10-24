using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace PROG6212_CMCS.Models
{
    public enum RoleName
    {
        Lecturer,
        ProgramCoordinator,
        Manager,
        HR
    };

    // Inherits from IdentityUser with int as the primary key type
    public class User 
    {

        public int UserID { get; set; }
        // --- Data Properties ---
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public string Email { get; set; } = "";
        public string Password { get; set; } = "";
        public RoleName Role { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime DateCreated { get; set; } = DateTime.Now;

        [Required]
        [Phone]
        [StringLength(15)]
        public string CellNumber { get; set; } = "0000000000";

        // Note: No navigation collections needed here unless you want to access
        // all claims submitted by a user directly from the User object.
    }
}