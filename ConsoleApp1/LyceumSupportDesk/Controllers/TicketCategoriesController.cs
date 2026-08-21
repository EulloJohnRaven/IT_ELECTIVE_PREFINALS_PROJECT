using LyceumSupportDesk.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

public class TicketCategoriesController : Controller
{
    private readonly LycevmDbContext _context;

    public TicketCategoriesController(LycevmDbContext context)
    {
        _context = context;
    }

    // GET: TicketCategories
    public async Task<IActionResult> Index()
    {
        var categories = await _context.TicketCategories
            .Include(c => c.ParentCategory)
            .Include(c => c.Subcategories)
            .ToListAsync();
        return View(categories);
    }

    // GET: TicketCategories/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();

        var category = await _context.TicketCategories
            .Include(c => c.ParentCategory)
            .Include(c => c.Subcategories)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (category == null) return NotFound();

        return View(category);
    }

    // GET: TicketCategories/Create
    public async Task<IActionResult> Create()
    {
        ViewBag.ParentCategoryId = new SelectList(await _context.TicketCategories.ToListAsync(), "Id", "Name");
        return View();
    }

    // POST: TicketCategories/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Name,Description,ParentCategoryId")] TicketCategory ticketCategory)
    {
        if (ModelState.IsValid)
        {
            _context.Add(ticketCategory);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        ViewBag.ParentCategoryId = new SelectList(await _context.TicketCategories.ToListAsync(), "Id", "Name", ticketCategory.ParentCategoryId);
        return View(ticketCategory);
    }

    // GET: TicketCategories/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();

        var ticketCategory = await _context.TicketCategories.FindAsync(id);
        if (ticketCategory == null) return NotFound();

        // Prevent selecting itself as a parent category to avoid circular references
        ViewBag.ParentCategoryId = new SelectList(await _context.TicketCategories.Where(c => c.Id != id).ToListAsync(), "Id", "Name", ticketCategory.ParentCategoryId);
        return View(ticketCategory);
    }

    // POST: TicketCategories/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,ParentCategoryId")] TicketCategory ticketCategory)
    {
        if (id != ticketCategory.Id) return NotFound();

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(ticketCategory);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.TicketCategories.Any(e => e.Id == ticketCategory.Id))
                    return NotFound();
                else
                    throw;
            }
            return RedirectToAction(nameof(Index));
        }
        ViewBag.ParentCategoryId = new SelectList(await _context.TicketCategories.Where(c => c.Id != id).ToListAsync(), "Id", "Name", ticketCategory.ParentCategoryId);
        return View(ticketCategory);
    }
}