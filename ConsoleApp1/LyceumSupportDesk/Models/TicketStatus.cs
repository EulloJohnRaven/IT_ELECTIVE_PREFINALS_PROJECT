using System.ComponentModel.DataAnnotations;

public class TicketStatus
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(50)]
    public string Name { get; set; } = string.Empty;

    [Required]
    public bool IsClosed { get; set; } = false;

    public ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
}