using LyceumSupportDesk.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Ticket
{
    [Key]
    public int Id { get; set; }

    [Required]
    public int CustomerId { get; set; }

    [Required]
    public int CategoryId { get; set; }

    [Required]
    public int PriorityId { get; set; }

    [Required]
    public int StatusId { get; set; }

    [Required]
    [StringLength(200)]
    public string Subject { get; set; } = string.Empty;

    [Required]
    public string Description { get; set; } = string.Empty;

    [Required]
    public string CreatedAt { get; set; } = string.Empty;

    [Required]
    public string UpdatedAt { get; set; } = string.Empty;

    public string? DueAt { get; set; }
    public string? ResolvedAt { get; set; }
    public string? ClosedAt { get; set; }

    // Navigation Properties
    [ForeignKey("CustomerId")]
    public Customer? Customer { get; set; }

    [ForeignKey("CategoryId")]
    public TicketCategory? Category { get; set; }

    [ForeignKey("PriorityId")]
    public TicketPriority? Priority { get; set; }

    [ForeignKey("StatusId")]
    public TicketStatus? Status { get; set; }

    public ICollection<TicketAssignment> TicketAssignments { get; set; } = new List<TicketAssignment>();
    public ICollection<TicketComment> TicketComments { get; set; } = new List<TicketComment>();
    public ICollection<TicketAttachment> TicketAttachments { get; set; } = new List<TicketAttachment>();
    public ICollection<TicketTag> TicketTags { get; set; } = new List<TicketTag>();

    public ICollection<TicketNote> TicketNotes { get; set; } = new List<TicketNote>();
}