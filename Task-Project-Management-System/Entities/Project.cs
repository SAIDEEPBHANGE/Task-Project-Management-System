using Microsoft.EntityFrameworkCore;
using Shared.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Task_Project_Management_System.Entities
{
    [Index(nameof(OrganizationId), nameof(Slug), IsUnique = true)]
    public class Project : BaseEntity
    {
        // Foreign key to Organization
        [Required]
        public int OrganizationId { get; set; }

        [ForeignKey(nameof(OrganizationId))]
        public Organization Organization { get; set; } = null!;

        // Project title
        [Required]
        [MaxLength(150)]
        public string Title { get; set; } = null!;

        // Slug unique per Organization
        [Required]
        [MaxLength(100)]
        public string Slug { get; set; } = null!;

        // Optional description
        public string? Description { get; set; }

        // Status
        [Required]
        public ProjectStatus Status { get; set; } = ProjectStatus.Active;

        // Created by user
        [Required]
        public int CreatedById { get; set; }

        [ForeignKey(nameof(CreatedById))]
        public User CreatedBy { get; set; } = null!;

        // Optional project dates
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        // Navigation properties
        public ICollection<ProjectMember> Members { get; set; } = new List<ProjectMember>();
        public ICollection<TaskInfo> Tasks { get; set; } = new List<TaskInfo>();
    }
}
