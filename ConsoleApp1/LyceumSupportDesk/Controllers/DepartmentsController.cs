using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LyceumSupportDesk.Models;

public class DepartmentsController : Controller
{
    private readonly LycevmDbContext _context;

    public DepartmentsController(LycevmDbContext context)
    {
        _context = context;
    }

    // GET: Departments
    public async Task<IActionResult> Index()
    {
        var departments = await _context.Departments
            .Include(d => d.Employees)
            .Select(d => new DepartmentSummaryViewModel
            {
                Id = d.Id,
                Name = d.Name,
                EmployeeCount = d.Employees.Count,
                ActiveTicketsCount = _context.TicketAssignments
                    .Count(ta => ta.Employee.DepartmentId == d.Id &&
                                 ta.Ticket.Status.Name != "Resolved" &&
                                 ta.Ticket.Status.Name != "Closed")
            })
            .ToListAsync();

        return View(departments);
    }

    // GET: Departments/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();

        var department = await _context.Departments
            .Include(d => d.Employees)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (department == null) return NotFound();

        return View(department);
    }

    // GET: Departments/Workload/5
    public async Task<IActionResult> Workload(int? id)
    {
        if (id == null) return NotFound();

        var department = await _context.Departments
            .Include(d => d.Employees)
                .ThenInclude(e => e.TicketAssignments)
                    .ThenInclude(ta => ta.Ticket)
                        .ThenInclude(t => t.Status)
            .FirstOrDefaultAsync(d => d.Id == id);

        if (department == null) return NotFound();

        return View(department);
    }

    // GET: Departments/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: Departments/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Name")] Department department)
    {
        if (ModelState.IsValid)
        {
            _context.Add(department);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(department);
    }

    // GET: Departments/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();

        var department = await _context.Departments.FindAsync(id);
        if (department == null) return NotFound();

        return View(department);
    }

    // POST: Departments/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] Department department)
    {
        if (id != department.Id) return NotFound();

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(department);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Departments.Any(e => e.Id == department.Id))
                    return NotFound();
                else
                    throw;
            }
            return RedirectToAction(nameof(Index));
        }
        return View(department);
    }
}

public class DepartmentSummaryViewModel
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int EmployeeCount { get; set; }
    public int ActiveTicketsCount { get; set; }
}