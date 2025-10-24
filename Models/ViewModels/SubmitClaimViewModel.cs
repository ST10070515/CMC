// Inside PROG6212_CMCS.Models.ViewModels/SubmitClaimViewModel.cs

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

public class SubmitClaimViewModel
{
    // === Form Input Properties ===
    [Required]
    [Range(0.01, 1000)]
    public decimal HoursWorked { get; set; }

    [Required]
    [Range(10.00, 500.00)]
    public decimal HourlyRate { get; set; }

    [Required]
    public int CourseID { get; set; }

    [Required]
    public string Description { get; set; }

    public string AdditionalNotes { get; set; }

    // --- File Handling ---
    // NOTE: This property name must match the name in the [HttpPost] action
    public IFormFile[] Documents { get; set; }

    // === View Display Properties ===
    // Used to populate the dropdown in the [HttpGet]
    public List<SelectListItem> AvailableCourses { get; set; }

    // REMOVE the unnecessary properties (ClaimID, Amount, SubmissionDate) that were added for debugging purposes
}