using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LyceumSupportDesk.Models;
using LyceumSupportDesk.Models.ViewModels;

public class EmployeesController : Controller
{
    private readonly LycevmDbContext _context;

    public EmployeesController(LycevmDbContext context)
    {
        _context = context;
    }

    // GET: Employees/Workload
    public async Task<IActionResult> Workload(string searchString, int? departmentId)
    {
        // 1. Base Query grouping Employee data with Ticket Assignments
        var workloadQuery = _context.Employees
            .Include(e => e.Department)
            .Select(e => new EmployeeWorkloadViewModel
            {
                EmployeeId = e.Id,
                FullName = e.FirstName + " " + e.LastName,
                Email = e.Email,
                DepartmentName = e.Department != null ? e.Department.Name : "Unassigned",

                // Aggregate LINQ subqueries to count tickets based on status
                TotalTickets = _context.TicketAssignments.Count(ta => ta.EmployeeId == e.Id),

                // Assuming "Resolved" or "Closed" signify completed tickets. 
                // Adjust status strings if your DB uses different names (e.g., "Done")
                ResolvedTickets = _context.TicketAssignments
                    .Count(ta => ta.EmployeeId == e.Id &&
                                 (ta.Ticket.Status.Name == "Resolved" || ta.Ticket.Status.Name == "Closed")),

                ActiveTickets = _context.TicketAssignments
                    .Count(ta => ta.EmployeeId == e.Id &&
                                 ta.Ticket.Status.Name != "Resolved" && ta.Ticket.Status.Name != "Closed")
            });

        // 2. Apply Filters (Search by Name or Department)
        if (!string.IsNullOrWhiteSpace(searchString))
        {
            workloadQuery = workloadQuery.Where(w => w.FullName.Contains(searchString));
        }

        if (departmentId.HasValue && departmentId.Value > 0)
        {
            var dept = await _context.Departments.FindAsync(departmentId.Value);
            if (dept != null)
            {
                workloadQuery = workloadQuery.Where(w => w.DepartmentName == dept.Name);
            }
        }

        // 3. Order by heaviest active workload first
        var workloads = await workloadQuery
            .OrderByDescending(w => w.ActiveTickets)
            .ToListAsync();

        ViewBag.CurrentSearch = searchString;
        ViewBag.Departments = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(
            await _context.Departments.ToListAsync(), "Id", "Name", departmentId);

        return View(workloads);
    }
}