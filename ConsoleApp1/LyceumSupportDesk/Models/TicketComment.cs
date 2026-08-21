using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class TicketComment
{
    [Key]
    public int Id { get; set; }

    [Required]
    public int TicketId { get; set; }

    public int? EmployeeId { get; set; } // Nullable for system-generated comments

    [Required]
    public string Comment { get; set; } = string.Empty;

    [Required]
    public string CreatedAt { get; set; } = string.Empty;

    [Required]
    public bool IsInternal { get; set; } = false;

    // Navigation Properties
    [ForeignKey("TicketId")]
    public Ticket? Ticket { get; set; }

    [ForeignKey("EmployeeId")]
    public Employee? Employee { get; set; }
}