using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class TicketCategory
{
    [Key]
    public int Id { get; set; }

    public int? ParentCategoryId { get; set; }

    [Required]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    [ForeignKey("ParentCategoryId")]
    public TicketCategory? ParentCategory { get; set; }

    // Fixed: Lowercase 'c' in Subcategories to match your controllers
    public ICollection<TicketCategory> Subcategories { get; set; } = new List<TicketCategory>();

    public ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
}