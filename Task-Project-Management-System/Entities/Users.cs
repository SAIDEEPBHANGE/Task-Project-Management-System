using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;

namespace Task_Project_Management_System.Entities
{
    [Index(nameof(Username), IsUnique = true)]
    [Index(nameof(Email), IsUnique = true)]
    public class User : BaseEntity
    {
        // Full name
        [Required]
        [MaxLength(100)]
        public string FullName { get; set; } = null!;

        // Username (unique)
        [Required]
        [MaxLength(50)]
        public string Username { get; set; } = null!;

        // Email (unique)
        [Required]
        [MaxLength(100)]
        public string Email { get; set; } = null!;

        // Password hash
        [Required]
        [MaxLength(255)]
        public string PasswordHash { get; set; } = null!;

        // Optional avatar
        [MaxLength(255)]
        public string? AvatarUrl { get; set; }

        // Last login timestamp
        public DateTime? LastLoginAt { get; set; }

        // Navigation properties
        public ICollection<OrganizationMember> OrganizationMembers { get; set; } = new List<OrganizationMember>();
        public ICollection<ProjectMember> ProjectMembers { get; set; } = new List<ProjectMember>();
        public ICollection<Project> CreatedProjects { get; set; } = new List<Project>();
        public ICollection<Task> CreatedTasks { get; set; } = new List<Task>();
        public ICollection<Task> AssignedTasks { get; set; } = new List<Task>();
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
    }
}
