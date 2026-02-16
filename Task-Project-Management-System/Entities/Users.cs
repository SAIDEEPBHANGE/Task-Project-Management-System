using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Task_Project_Management_System.Entities
{
    [Index(nameof(Username), IsUnique = true)]
    [Index(nameof(Email), IsUnique = true)]
    public class User : BaseEntity
    {
        [Required]
        [MaxLength(100)]
        public string FullName { get; set; } = null!;

        [Required]
        [MaxLength(50)]
        public string Username { get; set; } = null!;

        [Required]
        [MaxLength(100)]
        public string Email { get; set; } = null!;

        [Required]
        [MaxLength(255)]
        public string PasswordHash { get; set; } = null!;

        [MaxLength(255)]
        public string? AvatarUrl { get; set; }

        public DateTime? LastLoginAt { get; set; }

        // --- Navigation Properties ---

        // Memberships (Join Tables)
        public virtual ICollection<OrganizationMember> OrganizationMembers { get; set; } = new List<OrganizationMember>();
        public virtual ICollection<ProjectMember> ProjectMembers { get; set; } = new List<ProjectMember>();

        // Relationships with Projects
        public virtual ICollection<Project> CreatedProjects { get; set; } = new List<Project>();

        // Relationships with Tasks (The source of your previous error)
        // EF Core needs to know which property maps to the TaskInfo.AssignedUser
        public virtual ICollection<TaskInfo> AssignedTasks { get; set; } = new List<TaskInfo>();

        // Optional: If TaskInfo has a 'CreatedBy' field
        public virtual ICollection<TaskInfo> CreatedTasks { get; set; } = new List<TaskInfo>();

        public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();
    }
}