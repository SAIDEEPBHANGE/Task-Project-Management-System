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
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options)
        {
        }

        // --- Core Tables ---
        public DbSet<User> Users { get; set; }
        public DbSet<Organization> Organizations { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<TaskInfo> Tasks { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<ActivityLog> ActivityLogs { get; set; }

        // --- Join Tables (Many-to-Many) ---
        public DbSet<OrganizationMember> OrganizationMembers { get; set; }
        public DbSet<ProjectMember> ProjectMembers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // 1. Join Table: OrganizationMember (Composite Key)
            modelBuilder.Entity<OrganizationMember>()
                .HasKey(om => new { om.OrganizationId, om.UserId });

            // Configure relationships for OrganizationMember
            modelBuilder.Entity<OrganizationMember>()
                .HasOne(om => om.User)
                .WithMany(u => u.OrganizationMembers)
                .HasForeignKey(om => om.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Keep Organization side generic in case Organization doesn't expose a Members collection
            modelBuilder.Entity<OrganizationMember>()
                .HasOne(om => om.Organization)
                .WithMany()
                .HasForeignKey(om => om.OrganizationId)
                .OnDelete(DeleteBehavior.Restrict);

            // 2. Join Table: ProjectMember (Composite Key)
            modelBuilder.Entity<ProjectMember>()
                .HasKey(pm => new { pm.ProjectId, pm.UserId });

            // Configure relationships for ProjectMember
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

            // 3. TaskInfo <-> User (Assigned Tasks)
            // TaskInfo has an 'AssignedTo' FK (int?) and 'AssignedUser' navigation
            modelBuilder.Entity<TaskInfo>()
                .HasOne(t => t.AssignedUser)
                .WithMany(u => u.AssignedTasks)
                .HasForeignKey(t => t.AssignedTo)
                .OnDelete(DeleteBehavior.Restrict);

            // 4. TaskInfo <-> User (Created Tasks)
            // TaskInfo has 'CreatedById' and 'CreatedBy' navigation
            modelBuilder.Entity<TaskInfo>()
                .HasOne(t => t.CreatedBy)
                .WithMany(u => u.CreatedTasks)
                .HasForeignKey(t => t.CreatedById)
                .OnDelete(DeleteBehavior.Restrict);

            // 5. Global Query Filters (Soft Delete Logic)
            // Ensures rows where DeletedAt is set are hidden by default for BaseEntity-derived types
            modelBuilder.Entity<User>().HasQueryFilter(u => u.DeletedAt == null);
            modelBuilder.Entity<Project>().HasQueryFilter(p => p.DeletedAt == null);
            modelBuilder.Entity<TaskInfo>().HasQueryFilter(t => t.DeletedAt == null);
            modelBuilder.Entity<OrganizationMember>().HasQueryFilter(om => om.DeletedAt == null);
            // Optional: if Organization/comment types inherit BaseEntity, uncomment:
            // modelBuilder.Entity<Organization>().HasQueryFilter(o => o.DeletedAt == null);
            // modelBuilder.Entity<Comment>().HasQueryFilter(c => c.DeletedAt == null);

            // 6. Global Delete Behavior 
            // Prevents cycles and multiple cascade paths
            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }
        }

        // 7. Automated Auditing (Timestamps)
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var entries = ChangeTracker.Entries()
                .Where(e => e.Entity is BaseEntity && (e.State == EntityState.Added || e.State == EntityState.Modified));

            foreach (var entityEntry in entries)
            {
                var entity = (BaseEntity)entityEntry.Entity;
                entity.UpdatedAt = DateTime.UtcNow;

                if (entityEntry.State == EntityState.Added)
                {
                    entity.CreatedAt = DateTime.UtcNow;
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }
    }
}