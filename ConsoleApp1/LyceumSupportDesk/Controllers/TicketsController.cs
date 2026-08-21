using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public class TicketsController : Controller
{
    private readonly LycevmDbContext _context;

    public TicketsController(LycevmDbContext context)
    {
        _context = context;
    }

    // GET: Tickets
    public async Task<IActionResult> Index()
    {
        var tickets = await _context.Ticket
            .Include(t => t.Customer)
            .Include(t => t.Category)
            .Include(t => t.Priority)
            .Include(t => t.Status)
            .ToListAsync();

        return View(tickets);
    }

    // GET: Tickets/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var ticket = await _context.Ticket
            .Include(t => t.Customer)
            .Include(t => t.Category)
            .Include(t => t.Priority)
            .Include(t => t.Status)
            .Include(t => t.TicketComments)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (ticket == null)
        {
            return NotFound();
        }

        return View(ticket);
    }
}