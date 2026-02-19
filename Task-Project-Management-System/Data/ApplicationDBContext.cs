using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Task_Project_Management_System.Entities;

namespace Task_Project_Management_System.Data
{
    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options)
            : base(options)
        {
        }

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

            // 1. OrganizationMember Configuration
            modelBuilder.Entity<OrganizationMember>(entity =>
            {
                entity.HasKey(om => new { om.OrganizationId, om.UserId });

                entity.HasOne(om => om.User)
                    .WithMany(u => u.OrganizationMembers)
                    .HasForeignKey(om => om.UserId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(om => om.Organization)
                    .WithMany() // Organizations don't necessarily need a list of members in the entity
                    .HasForeignKey(om => om.OrganizationId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasQueryFilter(om => om.DeletedAt == null && om.Organization.DeletedAt == null && om.User.DeletedAt == null);
            });

            // 2. ProjectMember Configuration
            modelBuilder.Entity<ProjectMember>(entity =>
            {
                entity.HasKey(pm => new { pm.ProjectId, pm.UserId });

                entity.HasOne(pm => pm.User)
                    .WithMany(u => u.ProjectMembers)
                    .HasForeignKey(pm => pm.UserId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(pm => pm.Project)
                    .WithMany(p => p.Members)
                    .HasForeignKey(pm => pm.ProjectId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasQueryFilter(pm => pm.DeletedAt == null && pm.Project.DeletedAt == null && pm.User.DeletedAt == null);
            });

            // 3. TaskInfo Configuration
            modelBuilder.Entity<TaskInfo>(entity =>
            {
                // Explicitly map to "Tasks" table if desired
                entity.ToTable("Tasks");

                entity.HasOne(t => t.AssignedUser)
                    .WithMany(u => u.AssignedTasks)
                    .HasForeignKey(t => t.AssignedTo)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(t => t.CreatedBy)
                    .WithMany(u => u.CreatedTasks)
                    .HasForeignKey(t => t.CreatedById)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasQueryFilter(t => t.DeletedAt == null);
            });

            // 4. Other Global Filters
            modelBuilder.Entity<User>().HasQueryFilter(u => u.DeletedAt == null);
            modelBuilder.Entity<Organization>().HasQueryFilter(o => o.DeletedAt == null);
            modelBuilder.Entity<Project>().HasQueryFilter(p => p.DeletedAt == null);
            modelBuilder.Entity<Comment>().HasQueryFilter(c => c.DeletedAt == null && c.TaskInfo.DeletedAt == null);

            // 5. Global Delete Behavior & Concurrency (Advanced Production Step)
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                // Set all Foreign Keys to Restrict
                foreach (var fk in entityType.GetForeignKeys())
                    fk.DeleteBehavior = DeleteBehavior.Restrict;
            }
        }

        // 6. Enhanced Automated Auditing
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

            var now = DateTime.UtcNow;

            foreach (var entry in entries)
            {
                var trackable = (BaseEntity)entry.Entity;

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
                    // Logic switch: Deleted -> Modified
                    entry.State = EntityState.Modified;
                    trackable.DeletedAt = now;
                    trackable.UpdatedAt = now;
                }
            }
        }
    }
}