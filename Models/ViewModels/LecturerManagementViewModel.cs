// In PROG6212_CMCS.Models.ViewModels/LecturerManagementViewModel.cs

using System.ComponentModel.DataAnnotations;

namespace PROG6212_CMCS.Models.ViewModels
{
    public class LecturerManagementViewModel
    {
        public int UserID { get; set; }

        [Required(ErrorMessage = "First Name is required.")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Last Name is required.")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email Address is required.")]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        // *** NEW FIELD: Cellphone Number ***
        [Required(ErrorMessage = "Cellphone Number is required.")]
        [Phone]
        [Display(Name = "Cellphone Number")]
        public string CellNumber { get; set; } = string.Empty;

        // This role is enforced by the controller but kept for clarity
        public string Role { get; set; } = "Lecturer";


    }
}