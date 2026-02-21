using System.ComponentModel.DataAnnotations;

namespace Shared.DTOs.Users
{
    public class UpdateUsersDto // Changed to public
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string FullName { get; set; } = null!;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;
        public string? AvatarUrl { get; set; }
    }
}