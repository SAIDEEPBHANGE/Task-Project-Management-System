using Microsoft.EntityFrameworkCore;
using Shared.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;

namespace Task_Project_Management_System.Entities
{
    [Index(nameof(ProjectId))]
    [Index(nameof(OrganizationId))]
    [Index(nameof(AssignedTo))]
    [Index(nameof(OrganizationId), nameof(Status))]
    public class Task : BaseEntity
    {
        // Foreign key to Project
        [Required]
        public int ProjectId { get; set; }

        [ForeignKey(nameof(ProjectId))]
        public Project Project { get; set; } = null!;

        // Foreign key to Organization (denormalized)
        [Required]
        public int OrganizationId { get; set; }

        [ForeignKey(nameof(OrganizationId))]
        public Organization Organization { get; set; } = null!;

        // Task title
        [Required]
        [MaxLength(150)]
        public string Title { get; set; } = null!;

        // Optional description
        public string? Description { get; set; }

        // Task status
        [Required]
        public TasksStatus Status { get; set; } = TasksStatus.Todo;

        // Task priority
        [Required]
        public TasksPriority Priority { get; set; } = TasksPriority.Medium;

        // Created by user
        [Required]
        public int CreatedById { get; set; }

        [ForeignKey(nameof(CreatedById))]
        public User CreatedBy { get; set; } = null!;

        // Assigned to user (optional)
        public int? AssignedTo { get; set; }

        [ForeignKey(nameof(AssignedTo))]
        public User? AssignedUser { get; set; }

        // Due date
        public DateTime? DueDate { get; set; }

        // Navigation properties
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
        public ICollection<ActivityLog> ActivityLogs { get; set; } = new List<ActivityLog>();
    }
}
