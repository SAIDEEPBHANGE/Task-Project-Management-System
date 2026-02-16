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

            // ===============================================
            // 1. OrganizationMember (Many-to-Many Mapping)
            // ===============================================
            modelBuilder.Entity<OrganizationMember>()
                .HasKey(om => new { om.OrganizationId, om.UserId });

            modelBuilder.Entity<OrganizationMember>()
                .HasOne(om => om.User)
                .WithMany(u => u.OrganizationMembers)
                .HasForeignKey(om => om.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<OrganizationMember>()
                .HasOne(om => om.Organization)
                .WithMany()
                .HasForeignKey(om => om.OrganizationId)
                .OnDelete(DeleteBehavior.Restrict);

            // ===============================================
            // 2. ProjectMember (Many-to-Many Mapping)
            // ===============================================
            modelBuilder.Entity<ProjectMember>()
                .HasKey(pm => new { pm.ProjectId, pm.UserId });

            modelBuilder.Entity<ProjectMember>()
                .HasOne(pm => pm.User)
                .WithMany(u => u.ProjectMembers)
                .HasForeignKey(pm => pm.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ProjectMember>()
                .HasOne(pm => pm.Project)
                .WithMany(p => p.Members)
                .HasForeignKey(pm => pm.ProjectId)
                .OnDelete(DeleteBehavior.Restrict);

            // ===============================================
            // 3. Task Relationships (Disambiguation)
            // ===============================================

            // Link Task to Assigned User
            modelBuilder.Entity<TaskInfo>()
                .HasOne(t => t.AssignedUser)
                .WithMany(u => u.AssignedTasks)
                .HasForeignKey(t => t.AssignedTo)
                .OnDelete(DeleteBehavior.Restrict);

            // Link Task to Creator
            modelBuilder.Entity<TaskInfo>()
                .HasOne(t => t.CreatedBy)
                .WithMany(u => u.CreatedTasks)
                .HasForeignKey(t => t.CreatedById)
                .OnDelete(DeleteBehavior.Restrict);

            // ===============================
            // 4. Global Query Filters (Soft Delete)
            // ===============================

            modelBuilder.Entity<User>()
                .HasQueryFilter(u => u.DeletedAt == null);

            modelBuilder.Entity<Organization>()
                .HasQueryFilter(o => o.DeletedAt == null);

            modelBuilder.Entity<Project>()
                .HasQueryFilter(p => p.DeletedAt == null);

            modelBuilder.Entity<TaskInfo>()
                .HasQueryFilter(t => t.DeletedAt == null);

            modelBuilder.Entity<Comment>()
                .HasQueryFilter(c => c.DeletedAt == null && c.TaskInfo.DeletedAt == null);

            modelBuilder.Entity<ProjectMember>()
                .HasQueryFilter(pm =>
                    pm.DeletedAt == null &&
                    pm.Project.DeletedAt == null &&
                    pm.User.DeletedAt == null);

            modelBuilder.Entity<OrganizationMember>()
                .HasQueryFilter(om =>
                    om.DeletedAt == null &&
                    om.Organization.DeletedAt == null &&
                    om.User.DeletedAt == null);

            // ===============================================
            // 5. Global Delete Behavior (Restrict)
            // ===============================================
            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }
        }

        // ===================================================
        // 6. Automated Auditing & Soft Delete Interception
        // ===================================================

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
            var entries = ChangeTracker.Entries();
            var now = DateTime.UtcNow;

            foreach (var entry in entries)
            {
                if (entry.Entity is BaseEntity trackable)
                {
                    switch (entry.State)
                    {
                        case EntityState.Added:
                            trackable.CreatedAt = now;
                            trackable.UpdatedAt = now;
                            trackable.DeletedAt = null;
                            break;

                        case EntityState.Modified:
                            trackable.UpdatedAt = now;
                            // Ensure CreatedAt is never modified
                            entry.Property(nameof(BaseEntity.CreatedAt)).IsModified = false;
                            break;

                        case EntityState.Deleted:
                            // Convert Hard Delete to Soft Delete
                            entry.State = EntityState.Modified;
                            trackable.DeletedAt = now;
                            trackable.UpdatedAt = now;
                            break;
                    }
                }
            }
        }
    }
}