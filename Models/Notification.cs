using System;
using System.ComponentModel.DataAnnotations;

namespace PROG6212_CMCS.Models
{
    public class Notification
    {
        [Key]
        public int NotificationID { get; set; }

        // --- Foreign Key ---
        public int UserID { get; set; }

        // --- Data Properties ---
        public string Message { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public bool IsRead { get; set; }

        // --- Navigation Property ---
        public User User { get; set; }
    }
}