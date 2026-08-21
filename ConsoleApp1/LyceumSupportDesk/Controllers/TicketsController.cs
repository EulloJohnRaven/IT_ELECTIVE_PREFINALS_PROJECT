using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

public class TicketsController : Controller
{
    private readonly LycevmDbContext _context;

    public TicketsController(LycevmDbContext context)
    {
        _context = context;
    }

    // GET: Tickets (with Search and Filtering)
    public async Task<IActionResult> Index(string searchString, int? statusId, int? priorityId)
    {
        var ticketsQuery = _context.Tickets
            .Include(t => t.Customer)
            .Include(t => t.Status)
            .Include(t => t.Priority)
            .AsQueryable();

        // Search Filter (Subject or Customer Company/Contact Name)
        if (!string.IsNullOrWhiteSpace(searchString))
        {
            ticketsQuery = ticketsQuery.Where(t =>
                t.Subject.Contains(searchString) ||
                t.Customer.CompanyName.Contains(searchString) ||
                t.Customer.ContactName.Contains(searchString));
        }

        // Status Filter
        if (statusId.HasValue && statusId.Value > 0)
        {
            ticketsQuery = ticketsQuery.Where(t => t.StatusId == statusId.Value);
        }

        // Priority Filter
        if (priorityId.HasValue && priorityId.Value > 0)
        {
            ticketsQuery = ticketsQuery.Where(t => t.PriorityId == priorityId.Value);
        }

        ViewBag.Statuses = new SelectList(await _context.TicketStatuses.ToListAsync(), "Id", "Name", statusId);
        ViewBag.Priorities = new SelectList(await _context.TicketPriorities.ToListAsync(), "Id", "Name", priorityId);
        ViewBag.CurrentSearch = searchString;

        var tickets = await ticketsQuery.OrderByDescending(t => t.Id).ToListAsync();
        return View(tickets);
    }

    // GET: Tickets/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var ticket = await _context.Tickets
            .Include(t => t.Customer)
            .Include(t => t.Status)
            .Include(t => t.Priority)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (ticket == null)
        {
            return NotFound();
        }

        ViewBag.Statuses = new SelectList(await _context.TicketStatuses.ToListAsync(), "Id", "Name", ticket.StatusId);
        return View(ticket);
    }

    // POST: Tickets/UpdateStatus
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateStatus(int id, int statusId)
    {
        var ticket = await _context.Tickets.FindAsync(id);
        if (ticket == null)
        {
            return NotFound();
        }

        ticket.StatusId = statusId;
        _context.Update(ticket);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Details), new { id = ticket.Id });
    }
}