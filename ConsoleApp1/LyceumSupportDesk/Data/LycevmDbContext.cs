using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

public class LycevmDbContext : DbContext
{
    public LycevmDbContext(DbContextOptions<LycevmDbContext> options) : base(options) { }

    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<Department> Departments => Set<Department>();
    public DbSet<Employee> Employees => Set<Employee>();
    public DbSet<Tag> Tags => Set<Tag>();
    public DbSet<Team> Teams => Set<Team>();
    public DbSet<TeamMember> TeamMembers => Set<TeamMember>();
    public DbSet<Tickets> Ticket => Set<Ticket>();
    public DbSet<TicketAssignment> TicketAssignments => Set<TicketAssignment>();
    public DbSet<TicketAttachment> TicketAttachments => Set<TicketAttachment>();
    public DbSet<TicketCategory> TicketCategories => Set<TicketCategory>();
    public DbSet<TicketComment> TicketComments => Set<TicketComment>();
    public DbSet<TicketPriority> TicketPriorities => Set<TicketPriority>();
    public DbSet<TicketStatus> TicketStatuses => Set<TicketStatus>();
    public DbSet<TicketTag> TicketTags => Set<TicketTag>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // 1. TeamMembers Composite Primary Key (TeamId, EmployeeId)
        modelBuilder.Entity<TeamMember>()
            .HasKey(tm => new { tm.TeamId, tm.EmployeeId });

        // 2. TicketAssignments Composite Primary Key (TicketId, EmployeeId)
        modelBuilder.Entity<TicketAssignment>()
            .HasKey(ta => new { ta.TicketId, ta.EmployeeId });

        // 3. Unique Index constraint for Teams (DepartmentId, Name)
        modelBuilder.Entity<Team>()
            .HasIndex(t => new { t.DepartmentId, t.Name })
            .IsUnique();
    }
}