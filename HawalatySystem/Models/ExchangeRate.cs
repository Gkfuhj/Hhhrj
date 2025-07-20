using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HawalatySystem.Models
{
    public class ExchangeRate
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(10)]
        public string FromCurrency { get; set; } = string.Empty;

        [Required]
        [StringLength(10)]
        public string ToCurrency { get; set; } = string.Empty;

        [Required]
        [Column(TypeName = "decimal(10,4)")]
        public decimal Rate { get; set; }

        [Column(TypeName = "decimal(10,4)")]
        public decimal BuyRate { get; set; }

        [Column(TypeName = "decimal(10,4)")]
        public decimal SellRate { get; set; }

        public DateTime Date { get; set; } = DateTime.Now.Date;

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public int CreatedByUserId { get; set; }

        public bool IsActive { get; set; } = true;

        [StringLength(200)]
        public string Notes { get; set; } = string.Empty;

        // Navigation properties
        public virtual User CreatedByUser { get; set; } = null!;
    }
}