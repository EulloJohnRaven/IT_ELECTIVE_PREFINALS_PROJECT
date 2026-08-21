using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public class CustomersController : Controller
{
    private readonly LycevmDbContext _context;

    public CustomersController(LycevmDbContext context)
    {
        _context = context;
    }

    // GET: Customers
    public async Task<IActionResult> Index()
    {
        var customers = await _context.Customers.ToListAsync();
        return View(customers);
    }

    // GET: Customers/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var customer = await _context.Customers
            .Include(c => c.Tickets)
                .ThenInclude(t => t.Status)
            .Include(c => c.Tickets)
                .ThenInclude(t => t.Priority)
            .FirstOrDefaultAsync(c => c.Id == id);

        if (customer == null)
        {
            return NotFound();
        }

        return View(customer);
    }
}