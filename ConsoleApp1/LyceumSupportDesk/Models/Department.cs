using System.ComponentModel.DataAnnotations;

public class Department
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    [Required]
    public bool IsActive { get; set; } = true;

    // Navigation properties for One-to-Many relationships
    public ICollection<Employee> Employees { get; set; } = new List<Employee>();
    public ICollection<Team> Teams { get; set; } = new List<Team>();
}