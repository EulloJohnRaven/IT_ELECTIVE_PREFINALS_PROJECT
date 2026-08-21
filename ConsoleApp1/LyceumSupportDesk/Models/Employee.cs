using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Employee
{
    [Key]
    public int Id { get; set; }

    [Required]
    public int DepartmentId { get; set; }

    [Required]
    [StringLength(50)]
    public string FirstName { get; set; } = string.Empty;

    [Required]
    [StringLength(50)]
    public string LastName { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    [StringLength(100)]
    public string Email { get; set; } = string.Empty;

    [Required]
    [StringLength(100)]
    public string JobTitle { get; set; } = string.Empty;

    [Required]
    public string HireDate { get; set; } = string.Empty;

    [Required]
    public bool IsActive { get; set; } = true;

    // Navigation properties
    [ForeignKey("DepartmentId")]
    public Department? Department { get; set; }

    public ICollection<TeamMember> TeamMembers { get; set; } = new List<TeamMember>();
    public ICollection<TicketAssignment> TicketAssignments { get; set; } = new List<TicketAssignment>();
    public ICollection<TicketComment> TicketComments { get; set; } = new List<TicketComment>();
}