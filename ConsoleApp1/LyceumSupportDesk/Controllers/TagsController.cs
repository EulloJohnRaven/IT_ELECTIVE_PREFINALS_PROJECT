using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LyceumSupportDesk.Models;

public class TagsController : Controller
{
    private readonly LycevmDbContext _context;

    public TagsController(LycevmDbContext context)
    {
        _context = context;
    }

    // GET: Tags
    public async Task<IActionResult> Index()
    {
        var tags = await _context.Tags
            .Include(t => t.TicketTags)
            .ToListAsync();
        return View(tags);
    }

    // GET: Tags/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: Tags/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Name,Description")] Tag tag)
    {
        if (ModelState.IsValid)
        {
            _context.Add(tag);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(tag);
    }

    // GET: Tags/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();

        var tag = await _context.Tags.FindAsync(id);
        if (tag == null) return NotFound();

        return View(tag);
    }

    // POST: Tags/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description")] Tag tag)
    {
        if (id != tag.Id) return NotFound();

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(tag);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Tags.Any(e => e.Id == tag.Id))
                    return NotFound();
                else
                    throw;
            }
            return RedirectToAction(nameof(Index));
        }
        return View(tag);
    }
}