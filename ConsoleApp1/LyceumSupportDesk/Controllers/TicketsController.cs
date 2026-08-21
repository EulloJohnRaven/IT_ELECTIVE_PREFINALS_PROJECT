using LyceumSupportDesk.Models;
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

    // GET: Tickets/Unassigned
    public async Task<IActionResult> Unassigned()
    {
        var unassignedTickets = await _context.Tickets
            .Include(t => t.Customer)
            .Include(t => t.Status)
            .Include(t => t.Priority)
            .Include(t => t.Category)
            .Where(t => !t.TicketAssignments.Any())
            .OrderByDescending(t => t.Id)
            .ToListAsync();

        ViewBag.EmployeeList = new SelectList(
            await _context.Employees.Select(e => new { Id = e.Id, FullName = e.FirstName + " " + e.LastName }).ToListAsync(),
            "Id",
            "FullName"
        );

        return View(unassignedTickets);
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

    // POST: Tickets/Assign
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Assign(int ticketId, int employeeId)
    {
        var ticket = await _context.Tickets.FindAsync(ticketId);
        var employee = await _context.Employees.FindAsync(employeeId);

        if (ticket == null || employee == null)
        {
            return NotFound();
        }

        // Check if an assignment already exists
        var existingAssignment = await _context.TicketAssignments
            .FirstOrDefaultAsync(ta => ta.TicketId == ticketId && ta.EmployeeId == employeeId);

        if (existingAssignment == null)
        {
            var assignment = new TicketAssignment
            {
                TicketId = ticketId,
                EmployeeId = employeeId,
                AssignedDate = DateTime.Now
            };

            _context.TicketAssignments.Add(assignment);
            await _context.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Unassigned));
    }

    // GET: Tickets/Details/5 (Updated for Multiple Assignees)
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();

        var ticket = await _context.Tickets
            .Include(t => t.Customer)
            .Include(t => t.Status)
            .Include(t => t.Priority)
            .Include(t => t.Category)
            .Include(t => t.TicketAssignments)
                .ThenInclude(ta => ta.Employee)
                    .ThenInclude(e => e.Department)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (ticket == null) return NotFound();

        ViewBag.Statuses = new SelectList(await _context.TicketStatuses.ToListAsync(), "Id", "Name", ticket.StatusId);

        // Pass a list of all employees for the multi-assign dropdown
        ViewBag.EmployeeList = new SelectList(
            await _context.Employees.Select(e => new { Id = e.Id, FullName = e.FirstName + " " + e.LastName }).ToListAsync(),
            "Id",
            "FullName"
        );

        return View(ticket);
    }

    // POST: Tickets/AddAssignment
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddAssignment(int ticketId, int employeeId)
    {
        var ticket = await _context.Tickets.FindAsync(ticketId);
        var employee = await _context.Employees.FindAsync(employeeId);

        if (ticket == null || employee == null) return NotFound();

        var existingAssignment = await _context.TicketAssignments
            .FirstOrDefaultAsync(ta => ta.TicketId == ticketId && ta.EmployeeId == employeeId);

        if (existingAssignment == null)
        {
            _context.TicketAssignments.Add(new TicketAssignment
            {
                TicketId = ticketId,
                EmployeeId = employeeId,
                AssignedDate = DateTime.Now
            });
            await _context.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Details), new { id = ticketId });
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
}