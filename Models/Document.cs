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
        public Claim Claim { get; set; } = new Claim();

        // --- Data Properties ---
        public string FileName { get; set; } = "";
        public string StoragePath { get; set; } = "";
        public string FileType { get; set; } = "";

    }
}