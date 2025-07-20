using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HawalatySystem.Models
{
    public class Transfer
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string TransferId { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string SenderName { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string ReceiverName { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string FromCountry { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string ToCountry { get; set; } = string.Empty;

        [Required]
        [StringLength(10)]
        public string Currency { get; set; } = string.Empty;

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Commission { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal FinalAmount { get; set; }

        [StringLength(50)]
        public string TransferMethod { get; set; } = string.Empty;

        [Required]
        public TransferStatus Status { get; set; }

        [StringLength(500)]
        public string Notes { get; set; } = string.Empty;

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public DateTime? CompletedDate { get; set; }

        public int CreatedByUserId { get; set; }

        public int? CompletedByUserId { get; set; }

        [StringLength(20)]
        public string SenderPhone { get; set; } = string.Empty;

        [StringLength(20)]
        public string ReceiverPhone { get; set; } = string.Empty;

        [Column(TypeName = "decimal(10,4)")]
        public decimal ExchangeRate { get; set; }

        // Navigation properties
        public virtual User CreatedByUser { get; set; } = null!;
        public virtual User? CompletedByUser { get; set; }
        public virtual Agent? FromAgent { get; set; }
        public virtual Agent? ToAgent { get; set; }

        public int? FromAgentId { get; set; }
        public int? ToAgentId { get; set; }
    }

    public enum TransferStatus
    {
        Pending = 0,
        Completed = 1,
        Cancelled = 2,
        InProgress = 3
    }
}