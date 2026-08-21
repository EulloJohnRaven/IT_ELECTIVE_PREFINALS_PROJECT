using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LyceumSupportDesk.Models;
public class CustomersController : Controller

{

    private readonly LycevmDbContext _context;



    public CustomersController(LycevmDbContext context)

    {

        _context = context;

    }



    // GET: Customers

    public async Task<IActionResult> Index(string searchString)

    {

        var customersQuery = _context.Customers.AsQueryable();



        if (!string.IsNullOrWhiteSpace(searchString))

        {

            customersQuery = customersQuery.Where(c =>

            c.CompanyName.Contains(searchString) ||

            c.ContactName.Contains(searchString) ||

            c.Email.Contains(searchString));

        }



        ViewBag.CurrentSearch = searchString;

        var customers = await customersQuery.OrderBy(c => c.CompanyName).ToListAsync();

        return View(customers);

    }



    // GET: Customers/Details/5

    public async Task<IActionResult> Details(int? id)

    {

        if (id == null) return NotFound();



        var customer = await _context.Customers

        .Include(c => c.Tickets)

        .ThenInclude(t => t.Status)

        .Include(c => c.Tickets)

        .ThenInclude(t => t.Priority)

        .FirstOrDefaultAsync(m => m.Id == id);



        if (customer == null) return NotFound();



        return View(customer);

    }



    // GET: Customers/Create

    public IActionResult Create()

    {

        return View();

    }



    // POST: Customers/Create

    [HttpPost]

    [ValidateAntiForgeryToken]

    public async Task<IActionResult> Create([Bind("CompanyName,ContactName,Email,Phone,IsActive")] Customer customer)

    {

        // 1. Automatically generate the CreatedAt timestamp behind the scenes

        customer.CreatedAt = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");



        // 2. Tell the validator to ignore backend-only fields so it doesn't block the save

        ModelState.Remove("CreatedAt");

        ModelState.Remove("Tickets");



        if (ModelState.IsValid)

        {

            _context.Add(customer);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index)); // Redirects back to the list on success

        }



        return View(customer);

    }



    // GET: Customers/Edit/5

    public async Task<IActionResult> Edit(int? id)

    {

        if (id == null) return NotFound();



        var customer = await _context.Customers.FindAsync(id);

        if (customer == null) return NotFound();



        return View(customer);

    }



    // POST: Customers/Edit/5

    [HttpPost]

    [ValidateAntiForgeryToken]

    public async Task<IActionResult> Edit(int id, [Bind("Id,CompanyName,ContactName,Email,Phone,IsActive")] Customer customer)

    {

        if (id != customer.Id) return NotFound();



        if (ModelState.IsValid)

        {

            try

            {

                _context.Update(customer);

                await _context.SaveChangesAsync();

            }

            catch (DbUpdateConcurrencyException)

            {

                if (!_context.Customers.Any(e => e.Id == customer.Id))

                    return NotFound();

                else

                    throw;

            }

            return RedirectToAction(nameof(Index));

        }

        return View(customer);

    }

}

