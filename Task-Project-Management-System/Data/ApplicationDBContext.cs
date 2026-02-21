using Microsoft.EntityFrameworkCore;
using Task_Project_Management_System.Entities;

namespace Task_Project_Management_System.Data
{
    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options)
            : base(options) { }

        // --- Core Tables ---
        public DbSet<User> Users { get; set; }
        public DbSet<Organization> Organizations { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<TaskInfo> Tasks { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<ActivityLog> ActivityLogs { get; set; }

        // --- Join Tables ---
        public DbSet<OrganizationMember> OrganizationMembers { get; set; }
        public DbSet<ProjectMember> ProjectMembers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<OrganizationMember>(entity =>
            {
                entity.HasKey(om => new { om.OrgId, om.UserId });

                entity.HasOne(om => om.Organization)
                    .WithMany(o => o.Members)
                    .HasForeignKey(om => om.OrgId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(om => om.User)
                    .WithMany(u => u.OrganizationMembers)
                    .HasForeignKey(om => om.UserId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasQueryFilter(om => om.DeletedAt == null && om.Organization.DeletedAt == null);
            });

            modelBuilder.Entity<ProjectMember>(entity =>
            {
                entity.HasKey(pm => new { pm.ProjectId, pm.UserId });

                entity.HasOne(pm => pm.Project)
                    .WithMany(p => p.Members)
                    .HasForeignKey(pm => pm.ProjectId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(pm => pm.User)
                    .WithMany(u => u.ProjectMembers)
                    .HasForeignKey(pm => pm.UserId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasQueryFilter(pm => pm.DeletedAt == null && pm.Project.DeletedAt == null);
            });

            modelBuilder.Entity<Project>(entity =>
            {
                entity.HasOne(p => p.CreatedBy)
                    .WithMany(u => u.CreatedProjects)
                    .HasForeignKey(p => p.CreatedById)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(p => p.Organization)
                    .WithMany(o => o.Projects)
                    .HasForeignKey(p => p.OrganizationId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<TaskInfo>(entity =>
            {
                entity.ToTable("Tasks");

                entity.HasOne(t => t.CreatedBy)
                    .WithMany(u => u.CreatedTasks)
                    .HasForeignKey(t => t.CreatedById)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(t => t.AssignedUser)
                    .WithMany(u => u.AssignedTasks)
                    .HasForeignKey(t => t.AssignedTo)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasQueryFilter(t => t.DeletedAt == null);
            });

            modelBuilder.Entity<User>().HasQueryFilter(u => u.DeletedAt == null);
            modelBuilder.Entity<Organization>().HasQueryFilter(o => o.DeletedAt == null);
            modelBuilder.Entity<Project>().HasQueryFilter(p => p.DeletedAt == null);
            modelBuilder.Entity<Comment>().HasQueryFilter(c => c.DeletedAt == null);

            modelBuilder.Entity<ActivityLog>()
                .HasQueryFilter(al => al.Organization.DeletedAt == null);

            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }
        }

        // --- Audit Logic ---
        public override int SaveChanges()
        {
            ApplyAuditLogic();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            ApplyAuditLogic();
            return base.SaveChangesAsync(cancellationToken);
        }

        private void ApplyAuditLogic()
        {
            var entries = ChangeTracker.Entries()
                .Where(e => e.Entity is BaseEntity &&
                (e.State == EntityState.Added || e.State == EntityState.Modified || e.State == EntityState.Deleted));

            foreach (var entry in entries)
            {
                var trackable = (BaseEntity)entry.Entity;
                var now = DateTime.UtcNow;

                if (entry.State == EntityState.Added)
                {
                    trackable.CreatedAt = now;
                    trackable.UpdatedAt = now;
                }
                else if (entry.State == EntityState.Modified)
                {
                    trackable.UpdatedAt = now;
                    entry.Property(nameof(BaseEntity.CreatedAt)).IsModified = false;
                }
                else if (entry.State == EntityState.Deleted)
                {
                    entry.State = EntityState.Modified;
                    trackable.DeletedAt = now;
                }
            }
        }
    }
}