using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LyceumSupportDesk.Models;

public class TicketsController : Controller
{
    private readonly LycevmDbContext _context;

    public TicketsController(LycevmDbContext context)
    {
        _context = context;
    }

    // GET: Tickets
    public async Task<IActionResult> Index(string searchString, int? statusId, int? priorityId)
    {
        var ticketsQuery = _context.Tickets
            .Include(t => t.Customer)
            .Include(t => t.Category)
            .Include(t => t.Priority)
            .Include(t => t.Status)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(searchString))
        {
            ticketsQuery = ticketsQuery.Where(t =>
                t.Subject.Contains(searchString) ||
                t.Description.Contains(searchString) ||
                t.Customer.CompanyName.Contains(searchString));
        }

        if (statusId.HasValue)
        {
            ticketsQuery = ticketsQuery.Where(t => t.StatusId == statusId.Value);
        }

        if (priorityId.HasValue)
        {
            ticketsQuery = ticketsQuery.Where(t => t.PriorityId == priorityId.Value);
        }

        ViewBag.CurrentSearch = searchString;
        ViewBag.Statuses = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.TicketStatuses, "Id", "Name", statusId);
        ViewBag.Priorities = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.TicketPriorities, "Id", "Name", priorityId);

        var tickets = await ticketsQuery.OrderByDescending(t => t.Id).ToListAsync();
        return View(tickets);
    }

    // GET: Tickets/Unassigned
    public async Task<IActionResult> Unassigned()
    {
        var unassignedTickets = await _context.Tickets
            .Include(t => t.Customer)
            .Include(t => t.Category)
            .Include(t => t.Priority)
            .Include(t => t.Status)
            .Where(t => !t.TicketAssignments.Any())
            .ToListAsync();

        ViewBag.EmployeeList = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.Employees, "Id", "LastName");
        return View(unassignedTickets);
    }

    // GET: Tickets/Create
    public IActionResult Create()
    {
        // Load data for the dropdown menus in the view
        ViewBag.CustomerId = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.Customers, "Id", "CompanyName");
        ViewBag.CategoryId = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.TicketCategories, "Id", "Name");
        ViewBag.PriorityId = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.TicketPriorities, "Id", "Name");
        ViewBag.StatusId = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.TicketStatuses, "Id", "Name");

        return View();
    }

    // POST: Tickets/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("CustomerId,CategoryId,PriorityId,StatusId,Subject,Description")] Ticket ticket)
    {
        // 1. Automatically set the timestamps
        ticket.CreatedAt = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        ticket.UpdatedAt = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

        // 2. Tell the validator to ignore backend-only fields so it doesn't block the save
        ModelState.Remove("CreatedAt");
        ModelState.Remove("UpdatedAt");
        ModelState.Remove("Customer");
        ModelState.Remove("Category");
        ModelState.Remove("Priority");
        ModelState.Remove("Status");

        if (ModelState.IsValid)
        {
            _context.Add(ticket);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index)); // Redirect to the ticket list!
        }

        // If something genuinely goes wrong, reload the dropdowns
        ViewBag.CustomerId = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.Customers, "Id", "CompanyName", ticket.CustomerId);
        ViewBag.CategoryId = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.TicketCategories, "Id", "Name", ticket.CategoryId);
        ViewBag.PriorityId = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.TicketPriorities, "Id", "Name", ticket.PriorityId);
        ViewBag.StatusId = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.TicketStatuses, "Id", "Name", ticket.StatusId);

        return View(ticket);
    }

    // GET: Tickets/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();

        var ticket = await _context.Tickets
            .Include(t => t.Customer)
            .Include(t => t.Category)
            .Include(t => t.Priority)
            .Include(t => t.Status)
            .Include(t => t.TicketAssignments)
                .ThenInclude(ta => ta.Employee)
                    .ThenInclude(e => e.Department)
            .Include(t => t.TicketNotes)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (ticket == null) return NotFound();

        ViewBag.Statuses = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.TicketStatuses, "Id", "Name", ticket.StatusId);
        ViewBag.EmployeeList = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.Employees, "Id", "LastName");

        return View(ticket);
    }

    // POST: Tickets/AddAssignment
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddAssignment(int ticketId, int employeeId)
    {
        if (!_context.TicketAssignments.Any(ta => ta.TicketId == ticketId && ta.EmployeeId == employeeId))
        {
            var assignment = new TicketAssignment
            {
                TicketId = ticketId,
                EmployeeId = employeeId,
                AssignedAt = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), // Automatically populate the required date
                IsPrimary = true
            };
            _context.TicketAssignments.Add(assignment);
            await _context.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Unassigned)); // Keep them in the unassigned queue or send to Details
    }

    // POST: Tickets/RemoveAssignment
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RemoveAssignment(int ticketId, int employeeId)
    {
        var assignment = await _context.TicketAssignments
            .FirstOrDefaultAsync(ta => ta.TicketId == ticketId && ta.EmployeeId == employeeId);

        if (assignment != null)
        {
            _context.TicketAssignments.Remove(assignment);
            await _context.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Details), new { id = ticketId });
    }

    // POST: Tickets/AddNote
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddNote(int ticketId, string content)
    {
        if (!string.IsNullOrWhiteSpace(content))
        {
            var note = new TicketNote
            {
                TicketId = ticketId,
                Content = content,
                CreatedAt = DateTime.UtcNow
            };
            _context.TicketNotes.Add(note);
            await _context.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Details), new { id = ticketId });
    }

    // POST: Tickets/UpdateStatus
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateStatus(int id, int statusId)
    {
        var ticket = await _context.Tickets.FindAsync(id);
        if (ticket != null)
        {
            ticket.StatusId = statusId;
            ticket.UpdatedAt = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"); // Ensure required timestamp is updated

            _context.Update(ticket);
            await _context.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Details), new { id });
    }
}