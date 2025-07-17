using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HawalatySystem.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Username { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string FullName { get; set; } = string.Empty;

        [Required]
        [StringLength(255)]
        public string PasswordHash { get; set; } = string.Empty;

        [StringLength(100)]
        public string Email { get; set; } = string.Empty;

        [StringLength(20)]
        public string Phone { get; set; } = string.Empty;

        [Required]
        public UserRole Role { get; set; }

        [Required]
        [StringLength(50)]
        public string Country { get; set; } = string.Empty;

        [StringLength(50)]
        public string City { get; set; } = string.Empty;

        public bool IsActive { get; set; } = true;

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public DateTime? LastLoginDate { get; set; }

        public int? AgentId { get; set; }

        // Navigation properties
        public virtual Agent? Agent { get; set; }
        public virtual ICollection<Transfer> CreatedTransfers { get; set; } = new List<Transfer>();
        public virtual ICollection<Transfer> CompletedTransfers { get; set; } = new List<Transfer>();
        public virtual ICollection<UserPermission> UserPermissions { get; set; } = new List<UserPermission>();
    }

    public enum UserRole
    {
        Admin = 0,
        Accountant = 1,
        Agent = 2,
        Manager = 3,
        Viewer = 4
    }
}