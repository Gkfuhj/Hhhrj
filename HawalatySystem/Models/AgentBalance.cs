using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HawalatySystem.Models
{
    public class AgentBalance
    {
        [Key]
        public int Id { get; set; }

        public int AgentId { get; set; }

        [Required]
        [StringLength(10)]
        public string Currency { get; set; } = string.Empty;

        [Column(TypeName = "decimal(18,2)")]
        public decimal Balance { get; set; }

        public DateTime LastUpdated { get; set; } = DateTime.Now;

        // Navigation properties
        public virtual Agent Agent { get; set; } = null!;
    }
}