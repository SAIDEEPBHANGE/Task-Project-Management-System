using Microsoft.EntityFrameworkCore;
using Shared.Enums;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Task_Project_Management_System.Entities
{
    [Index(nameof(ProjectId), nameof(UserId), IsUnique = true)]
    public class ProjectMember : BaseEntity
    {
        // Foreign key to Project
        [Required]
        public int ProjectId { get; set; }

        [ForeignKey(nameof(ProjectId))]
        public Project Project { get; set; } = null!;

        // Foreign key to User
        [Required]
        public int UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public User User { get; set; } = null!;

        // Role in project
        [Required]
        public ProjectRole Role { get; set; } = ProjectRole.Member;

        // Joined timestamp
        [Required]
        public DateTime JoinedAt { get; set; } = DateTime.UtcNow;
    }
}
