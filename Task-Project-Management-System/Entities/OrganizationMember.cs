using Microsoft.EntityFrameworkCore;
using Shared.Enums;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Task_Project_Management_System.Entities
{
    public class OrganizationMember : BaseEntity
    {
        [Required]
        public int OrgId { get; set; }
        public Organization Organization { get; set; } = null!;

        [Required]
        public int UserId { get; set; }
        public User User { get; set; } = null!;

        [Required]
        public OrganizationRole Role { get; set; } = OrganizationRole.Member;

        [Required]
        public DateTime JoinedAt { get; set; } = DateTime.UtcNow;

        public DateTime? LeftAt { get; set; }
    }
}