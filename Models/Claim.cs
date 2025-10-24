using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PROG6212_CMCS.Models
{
    public enum ClaimStatus { 
        PENDING,
        APPROVED,
        REJECTED
    }
    public class Claim
    {
        [Key]
        public int ClaimID { get; set; }

        // --- Foreign Keys ---
        public int UserID { get; set; } 
        public User? User { get; set; } 

        // --- Data Properties ---
        [Column(TypeName = "decimal(5,2)")]
        public decimal HoursWorked { get; set; }

        [Column(TypeName = "decimal(5,2)")]
        public decimal HourlyRate { get; set; }

        [Column(TypeName = "decimal(7,2)")]
        public decimal ClaimAmount { get; set; }


        public string CourseName { get; set; } = "";
        public ClaimStatus ClaimStatus { get; set; } = ClaimStatus.PENDING;
        public string AdditionalNotes { get; set; } = "";
        public DateTime SubmissionDate { get; set; } = DateTime.Now;
        public DateTime LastUpdated { get; set; } = DateTime.Now;


    }
}