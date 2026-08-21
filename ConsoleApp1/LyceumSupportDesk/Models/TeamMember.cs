using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class TeamMember
{
    [Required]
    public int TeamId { get; set; }

    [Required]
    public int EmployeeId { get; set; }

    [Required]
    public string JoinedAt { get; set; } = string.Empty;

    // Navigation properties
    [ForeignKey("TeamId")]
    public Team? Team { get; set; }

    [ForeignKey("EmployeeId")]
    public Employee? Employee { get; set; }
}