using System.ComponentModel.DataAnnotations;

public class Tag
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(50)]
    public string Name { get; set; } = string.Empty;

    // Fixed: Added the missing Description property here!
    public string? Description { get; set; }

    public ICollection<TicketTag> TicketTags { get; set; } = new List<TicketTag>();
}