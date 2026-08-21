using Microsoft.EntityFrameworkCore;

namespace LyceumSupportDesk.Models
{
    public class LycevmDbContext : DbContext
    {
        public LycevmDbContext(DbContextOptions<LycevmDbContext> options)
            : base(options)
        {
        }

        // DbSets representing your database tables
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<TicketCategory> TicketCategories { get; set; }
        public DbSet<TicketStatus> TicketStatuses { get; set; }
        public DbSet<TicketPriority> TicketPriorities { get; set; }
        public DbSet<TicketAssignment> TicketAssignments { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<TicketTag> TicketTags { get; set; }
        public DbSet<TicketNote> TicketNotes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // 1. Configure Self-Referencing Relationship for TicketCategory
            // A category can have one Parent Category, and a Parent Category can have many Subcategories.
            modelBuilder.Entity<TicketCategory>()
                .HasOne(c => c.ParentCategory)
                .WithMany(c => c.Subcategories)
                .HasForeignKey(c => c.ParentCategoryId)
                .OnDelete(DeleteBehavior.Restrict); // Prevent cascading deletes that wipe out entire category trees

            // 2. Configure Composite Primary Key for TicketTag (Join Table)
            // The combination of TicketId and TagId must be unique.
            modelBuilder.Entity<TicketTag>()
                .HasKey(tt => new { tt.TicketId, tt.TagId });

            // Ensure the relationships for the TicketTag join table are explicitly defined
            modelBuilder.Entity<TicketTag>()
                .HasOne(tt => tt.Ticket)
                .WithMany(t => t.TicketTags)
                .HasForeignKey(tt => tt.TicketId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<TicketTag>()
                .HasOne(tt => tt.Tag)
                .WithMany(t => t.TicketTags)
                .HasForeignKey(tt => tt.TagId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}