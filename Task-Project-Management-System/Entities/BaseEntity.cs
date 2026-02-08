using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Task_Project_Management_System.Entities
{
    public abstract class BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        // Audit fields
        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }

        // Soft Delete field
        public DateTime? DeletedAt { get; set; }

        // Helper property to check if entity is soft-deleted
        [NotMapped]
        public bool IsDeleted => DeletedAt != null;
    }
}
