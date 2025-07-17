using System.ComponentModel.DataAnnotations;

namespace HawalatySystem.Models
{
    public class UserPermission
    {
        [Key]
        public int Id { get; set; }

        public int UserId { get; set; }

        [Required]
        [StringLength(50)]
        public string Permission { get; set; } = string.Empty;

        public bool IsGranted { get; set; } = true;

        // Navigation properties
        public virtual User User { get; set; } = null!;
    }
}