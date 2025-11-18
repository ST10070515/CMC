using System;
using System.ComponentModel.DataAnnotations;

namespace PROG6212_CMCS.Models
{
    public class Notification
    {
        [Key]
        public int NotificationID { get; set; }
        public int UserID { get; set; }
        public string Message { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public bool IsRead { get; set; }
        public User User { get; set; }
    }
}