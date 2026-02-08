using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Task_Project_Management_System.Entities
{
    public class Comment : BaseEntity
    {
        // Foreign key to Task
        [Required]
        public int TaskId { get; set; }

        [ForeignKey(nameof(TaskId))]
        public Task Task { get; set; } = null!;

        // Foreign key to User
        [Required]
        public int UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public User User { get; set; } = null!;

        // Optional parent comment (self-referencing)
        public int? ParentCommentId { get; set; }

        [ForeignKey(nameof(ParentCommentId))]
        public Comment? ParentComment { get; set; }

        public ICollection<Comment> Replies { get; set; } = new List<Comment>();

        // Comment content
        [Required]
        public string Content { get; set; } = null!;

        // Edited flag
        [Required]
        public bool IsEdited { get; set; } = false;
    }
}
