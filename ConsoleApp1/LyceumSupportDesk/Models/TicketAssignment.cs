using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class TicketAssignment
{
    [Required]
    public int TicketId { get; set; }

    [Required]
    public int EmployeeId { get; set; }

    [Required]
    public string AssignedAt { get; set; } = string.Empty;

    public string? UnassignedAt { get; set; }

    [Required]
    public bool IsPrimary { get; set; } = false;

    // Navigation Properties
    [ForeignKey("TicketId")]
    public Ticket? Ticket { get; set; }

    [ForeignKey("EmployeeId")]
    public Employee? Employee { get; set; }
}