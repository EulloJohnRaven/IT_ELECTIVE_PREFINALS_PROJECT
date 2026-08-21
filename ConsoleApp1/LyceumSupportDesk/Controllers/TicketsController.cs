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

    // GET: Tickets/Create
    public async Task<IActionResult> Create()
    {
        ViewData["CustomerId"] = new SelectList(await _context.Customers.Where(c => c.IsActive).ToListAsync(), "Id", "CompanyName");
        ViewData["CategoryId"] = new SelectList(await _context.TicketCategories.ToListAsync(), "Id", "Name");
        ViewData["PriorityId"] = new SelectList(await _context.TicketPriorities.OrderBy(p => p.SortOrder).ToListAsync(), "Id", "Name");
        ViewData["StatusId"] = new SelectList(await _context.TicketStatuses.ToListAsync(), "Id", "Name");

        return View();
    }

    // POST: Tickets/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Subject,Description,CustomerId,CategoryId,PriorityId,StatusId")] Ticket ticket)
    {
        if (ModelState.IsValid)
        {
            string currentTimestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            ticket.CreatedAt = currentTimestamp;
            ticket.UpdatedAt = currentTimestamp;

            _context.Add(ticket);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        ViewData["CustomerId"] = new SelectList(await _context.Customers.Where(c => c.IsActive).ToListAsync(), "Id", "CompanyName", ticket.CustomerId);
        ViewData["CategoryId"] = new SelectList(await _context.TicketCategories.ToListAsync(), "Id", "Name", ticket.CategoryId);
        ViewData["PriorityId"] = new SelectList(await _context.TicketPriorities.OrderBy(p => p.SortOrder).ToListAsync(), "Id", "Name", ticket.PriorityId);
        ViewData["StatusId"] = new SelectList(await _context.TicketStatuses.ToListAsync(), "Id", "Name", ticket.StatusId);

        return View(ticket);
    }

    // GET: Tickets/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var ticket = await _context.Ticket.FindAsync(id);
        if (ticket == null)
        {
            return NotFound();
        }

        ViewData["CustomerId"] = new SelectList(await _context.Customers.Where(c => c.IsActive).ToListAsync(), "Id", "CompanyName", ticket.CustomerId);
        ViewData["CategoryId"] = new SelectList(await _context.TicketCategories.ToListAsync(), "Id", "Name", ticket.CategoryId);
        ViewData["PriorityId"] = new SelectList(await _context.TicketPriorities.OrderBy(p => p.SortOrder).ToListAsync(), "Id", "Name", ticket.PriorityId);
        ViewData["StatusId"] = new SelectList(await _context.TicketStatuses.ToListAsync(), "Id", "Name", ticket.StatusId);

        return View(ticket);
    }

    // POST: Tickets/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Subject,Description,CustomerId,CategoryId,PriorityId,StatusId,CreatedAt")] Ticket ticket)
    {
        if (id != ticket.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                ticket.UpdatedAt = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                _context.Update(ticket);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TicketExists(ticket.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
        }

        ViewData["CustomerId"] = new SelectList(await _context.Customers.Where(c => c.IsActive).ToListAsync(), "Id", "CompanyName", ticket.CustomerId);
        ViewData["CategoryId"] = new SelectList(await _context.TicketCategories.ToListAsync(), "Id", "Name", ticket.CategoryId);
        ViewData["PriorityId"] = new SelectList(await _context.TicketPriorities.OrderBy(p => p.SortOrder).ToListAsync(), "Id", "Name", ticket.PriorityId);
        ViewData["StatusId"] = new SelectList(await _context.TicketStatuses.ToListAsync(), "Id", "Name", ticket.StatusId);

        return View(ticket);
    }

    // GET: Tickets/Delete/5
    public async Task<IActionResult> Delete(int? id)
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
            .FirstOrDefaultAsync(m => m.Id == id);

        if (ticket == null)
        {
            return NotFound();
        }

        return View(ticket);
    }

    // POST: Tickets/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var ticket = await _context.Ticket.FindAsync(id);
        if (ticket != null)
        {
            _context.Remove(ticket);
            await _context.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Index));
    }

    private bool TicketExists(int id)
    {
        return _context.Ticket.Any(e => e.Id == id);
    }
}