using System.ComponentModel.DataAnnotations;

public class TicketPriority
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(50)]
    public string Name { get; set; } = string.Empty;

    [Required]
    public int SortOrder { get; set; }

    [Required]
    public int ResponseHours { get; set; }

    public ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
}