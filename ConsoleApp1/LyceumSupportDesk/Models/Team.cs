using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Team
{
    [Key]
    public int Id { get; set; }

    [Required]
    public int DepartmentId { get; set; }

    [Required]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    // Navigation properties
    [ForeignKey("DepartmentId")]
    public Department? Department { get; set; }

    public ICollection<TeamMember> TeamMembers { get; set; } = new List<TeamMember>();
}