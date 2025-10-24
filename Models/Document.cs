using System;
using System.ComponentModel.DataAnnotations;

namespace PROG6212_CMCS.Models
{
    public class Document
    {
        [Key]
        public int DocumentID { get; set; }

        // --- Foreign Key (Fixing CS1061 errors) ---
        public int ClaimID { get; set; } = 0;
        public Claim? Claim { get; set; }

        // --- Data Properties ---
        public string FileName { get; set; } = "";
        public string ContentType { get; set; } = "";
        public byte[] FileData { get; set; } = [];

    }
}