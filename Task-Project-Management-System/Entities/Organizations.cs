using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Task_Project_Management_System.Entities
{
    [Index(nameof(Slug), IsUnique = true)]
    public class Organization : BaseEntity
    {
        // Organization Name
        [Required]
        [MaxLength(150)]
        public string Name { get; set; } = null!;

        // Unique Slug
        [Required]
        [MaxLength(100)]
        public string Slug { get; set; } = null!;

        // Owner foreign key
        [Required]
        public int OwnerId { get; set; }

        [ForeignKey(nameof(OwnerId))]
        public User Owner { get; set; } = null!;

        // Navigation properties
        public ICollection<OrganizationMember> Members { get; set; } = new List<OrganizationMember>();
        public ICollection<Project> Projects { get; set; } = new List<Project>();
    }
}
