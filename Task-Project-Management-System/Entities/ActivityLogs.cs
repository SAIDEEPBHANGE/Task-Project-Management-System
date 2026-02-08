using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Task_Project_Management_System.Entities
{
    public class ActivityLog
    {
        // Primary key (bigint)
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        // Foreign key to Organization
        [Required]
        public int OrganizationId { get; set; }

        [ForeignKey(nameof(OrganizationId))]
        public Organization Organization { get; set; } = null!;

        // Optional foreign key to User
        public int? UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public User? User { get; set; }

        // Action description
        [MaxLength(150)]
        public string? Action { get; set; }

        // Optional entity reference
        public int? EntityId { get; set; }

        // Optional old/new values
        public string? OldValue { get; set; }
        public string? NewValue { get; set; }

        // Timestamp
        [Required]
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }
}
