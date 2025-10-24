using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PROG6212_CMCS.Models
{
    public class Payment
    {
        // NOTE: ClaimID is often used as the PK in a one-to-one relationship
        [Key]
        public int PaymentID { get; set; } // Assuming a unique PaymentID separate from ClaimID

        // --- Foreign Keys (Fixing CS1061 errors) ---
        public int ClaimID { get; set; }
        public int ProcessedByID { get; set; }

        // --- Data Properties ---
        public DateTime PaymentDate { get; set; } = DateTime.Now;
        [Column(TypeName = "decimal(18,2)")]
        public decimal PaymentAmount { get; set; }
        public string PaymentReference { get; set; } = "";

        // --- Navigation Properties (Fixing CS1061 errors) ---
        public Claim Claim { get; set; }
        public User ProcessedBy { get; set; }
    }
}