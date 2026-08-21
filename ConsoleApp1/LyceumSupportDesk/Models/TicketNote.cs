using System;
using System.ComponentModel.DataAnnotations;

namespace LyceumSupportDesk.Models
{
    public class TicketNote
    {
        public int Id { get; set; }

        [Required]
        public int TicketId { get; set; }
        public Ticket? Ticket { get; set; }

        [Required]
        [Display(Name = "Note Content")]
        public string Content { get; set; } = string.Empty;

        [Display(Name = "Date Created")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}