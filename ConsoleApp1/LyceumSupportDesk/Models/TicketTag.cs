using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class TicketTag
{
    [Required]
    public int TicketId { get; set; }

    [Required]
    public int TagId { get; set; }

    [ForeignKey("TicketId")]
    public Ticket? Ticket { get; set; }

    [ForeignKey("TagId")]
    public Tag? Tag { get; set; }
}