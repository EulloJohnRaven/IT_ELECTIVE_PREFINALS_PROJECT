using Microsoft.EntityFrameworkCore;

public class LycevmDbContext : DbContext
{
    public LycevmDbContext(DbContextOptions<LycevmDbContext> options) : base(options) { }

    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<Department> Departments => Set<Department>();
    public DbSet<Employee> Employees => Set<Employee>();
    public DbSet<Tag> Tags => Set<Tag>();
    public DbSet<Team> Teams => Set<Team>();
    public DbSet<TeamMember> TeamMembers => Set<TeamMember>();
    public DbSet<Ticket> Tickets => Set<Ticket>();
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

        modelBuilder.Entity<TeamMember>()
            .HasKey(tm => new { tm.TeamId, tm.EmployeeId });

        modelBuilder.Entity<TicketAssignment>()
            .HasKey(ta => new { ta.TicketId, ta.EmployeeId });

        modelBuilder.Entity<Team>()
            .HasIndex(t => new { t.DepartmentId, t.Name })
            .IsUnique();
    }
}