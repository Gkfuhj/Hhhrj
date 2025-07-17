using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using HawalatySystem.Data;
using HawalatySystem.Models;

namespace HawalatySystem.Services
{
    public class TransferService
    {
        private readonly HawalatyDbContext _context;
        private readonly AuthenticationService _authService;

        public TransferService(HawalatyDbContext context, AuthenticationService authService)
        {
            _context = context;
            _authService = authService;
        }

        public async Task<Transfer?> CreateTransferAsync(Transfer transfer)
        {
            try
            {
                // Generate unique transfer ID
                transfer.TransferId = await GenerateTransferIdAsync();
                transfer.CreatedDate = DateTime.Now;
                transfer.CreatedByUserId = _authService.CurrentUser?.Id ?? 1;
                transfer.Status = TransferStatus.Pending;

                // Calculate final amount
                transfer.FinalAmount = transfer.Amount - transfer.Commission;

                _context.Transfers.Add(transfer);
                await _context.SaveChangesAsync();

                return transfer;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<bool> CompleteTransferAsync(int transferId, int? completedByAgentId = null)
        {
            try
            {
                var transfer = await _context.Transfers.FindAsync(transferId);
                if (transfer == null || transfer.Status != TransferStatus.Pending)
                    return false;

                transfer.Status = TransferStatus.Completed;
                transfer.CompletedDate = DateTime.Now;
                transfer.CompletedByUserId = _authService.CurrentUser?.Id;

                if (completedByAgentId.HasValue)
                {
                    transfer.ToAgentId = completedByAgentId.Value;
                }

                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> CancelTransferAsync(int transferId, string reason)
        {
            try
            {
                var transfer = await _context.Transfers.FindAsync(transferId);
                if (transfer == null || transfer.Status == TransferStatus.Completed)
                    return false;

                transfer.Status = TransferStatus.Cancelled;
                transfer.Notes = $"{transfer.Notes}\nCancelled: {reason}";
                
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<List<Transfer>> GetTransfersAsync(
            DateTime? fromDate = null,
            DateTime? toDate = null,
            string? currency = null,
            TransferStatus? status = null,
            string? country = null,
            int? agentId = null)
        {
            var query = _context.Transfers
                .Include(t => t.CreatedByUser)
                .Include(t => t.CompletedByUser)
                .Include(t => t.FromAgent)
                .Include(t => t.ToAgent)
                .AsQueryable();

            if (fromDate.HasValue)
                query = query.Where(t => t.CreatedDate >= fromDate.Value);

            if (toDate.HasValue)
                query = query.Where(t => t.CreatedDate <= toDate.Value.AddDays(1));

            if (!string.IsNullOrEmpty(currency))
                query = query.Where(t => t.Currency == currency);

            if (status.HasValue)
                query = query.Where(t => t.Status == status.Value);

            if (!string.IsNullOrEmpty(country))
                query = query.Where(t => t.FromCountry == country || t.ToCountry == country);

            if (agentId.HasValue)
                query = query.Where(t => t.FromAgentId == agentId || t.ToAgentId == agentId);

            return await query.OrderByDescending(t => t.CreatedDate).ToListAsync();
        }

        public async Task<Transfer?> GetTransferByIdAsync(string transferId)
        {
            return await _context.Transfers
                .Include(t => t.CreatedByUser)
                .Include(t => t.CompletedByUser)
                .Include(t => t.FromAgent)
                .Include(t => t.ToAgent)
                .FirstOrDefaultAsync(t => t.TransferId == transferId);
        }

        public async Task<TransferStatistics> GetStatisticsAsync(DateTime? fromDate = null, DateTime? toDate = null)
        {
            var query = _context.Transfers.AsQueryable();

            if (fromDate.HasValue)
                query = query.Where(t => t.CreatedDate >= fromDate.Value);

            if (toDate.HasValue)
                query = query.Where(t => t.CreatedDate <= toDate.Value.AddDays(1));

            var stats = new TransferStatistics();

            // Total transfers by status
            stats.TotalPending = await query.CountAsync(t => t.Status == TransferStatus.Pending);
            stats.TotalCompleted = await query.CountAsync(t => t.Status == TransferStatus.Completed);
            stats.TotalCancelled = await query.CountAsync(t => t.Status == TransferStatus.Cancelled);

            // Total amounts by currency
            stats.TotalIncomingTRY = await query
                .Where(t => t.Currency == "TRY" && t.FromCountry == "Turkey" && t.Status == TransferStatus.Completed)
                .SumAsync(t => t.Amount);

            stats.TotalOutgoingLYD = await query
                .Where(t => t.Currency == "LYD" && t.FromCountry == "Libya" && t.Status == TransferStatus.Completed)
                .SumAsync(t => t.Amount);

            stats.TotalCommission = await query
                .Where(t => t.Status == TransferStatus.Completed)
                .SumAsync(t => t.Commission);

            return stats;
        }

        private async Task<string> GenerateTransferIdAsync()
        {
            var date = DateTime.Now;
            var prefix = $"HT{date:yyMMdd}";
            
            var lastTransfer = await _context.Transfers
                .Where(t => t.TransferId.StartsWith(prefix))
                .OrderByDescending(t => t.TransferId)
                .FirstOrDefaultAsync();

            int sequence = 1;
            if (lastTransfer != null)
            {
                var lastSequence = lastTransfer.TransferId.Substring(prefix.Length);
                if (int.TryParse(lastSequence, out int lastSeq))
                {
                    sequence = lastSeq + 1;
                }
            }

            return $"{prefix}{sequence:D4}";
        }
    }

    public class TransferStatistics
    {
        public int TotalPending { get; set; }
        public int TotalCompleted { get; set; }
        public int TotalCancelled { get; set; }
        public decimal TotalIncomingTRY { get; set; }
        public decimal TotalOutgoingLYD { get; set; }
        public decimal TotalCommission { get; set; }
        public Dictionary<string, decimal> BalancesByCurrency { get; set; } = new();
    }
}