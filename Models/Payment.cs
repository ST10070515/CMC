using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PROG6212_CMCS.Models
{
    public class Payment
    {
        [Key]
        public int PaymentID { get; set; } 
        public int ClaimID { get; set; }
        public int ProcessedByID { get; set; }
        public DateTime PaymentDate { get; set; } = DateTime.Now;
        [Column(TypeName = "decimal(18,2)")]
        public decimal PaymentAmount { get; set; }
        public string PaymentReference { get; set; } = "";
        public Claim Claim { get; set; }
        public User ProcessedBy { get; set; }
    }
}