using Microsoft.EntityFrameworkCore;
using Shared.Enums;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Task_Project_Management_System.Entities
{
    [Index(nameof(OrganizationId), nameof(UserId), IsUnique = true)]
    public class OrganizationMember : BaseEntity
    {
        // Foreign key to Organization
        [Required]
        public int OrganizationId { get; set; }

        [ForeignKey(nameof(OrganizationId))]
        public Organization Organization { get; set; } = null!;

        // Foreign key to User
        [Required]
        public int UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public User User { get; set; } = null!;

        // Role in the organization
        [Required]
        public OrganizationRole Role { get; set; } = OrganizationRole.Member;

        // Membership timestamps
        [Required]
        public DateTime JoinedAt { get; set; } = DateTime.UtcNow;

        public DateTime? LeftAt { get; set; }
    }
}
