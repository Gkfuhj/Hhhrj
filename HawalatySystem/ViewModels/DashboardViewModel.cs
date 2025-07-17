using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using HawalatySystem.Data;
using HawalatySystem.Models;
using HawalatySystem.Services;
using HawalatySystem.Resources.Localization;

namespace HawalatySystem.ViewModels
{
    public class DashboardViewModel : INotifyPropertyChanged
    {
        private readonly TransferService _transferService;
        private readonly AuthenticationService _authService;
        private readonly LocalizationManager _localization;
        private readonly ReportService _reportService;

        private ObservableCollection<Transfer> _recentTransfers = new();
        private TransferStatistics _statistics = new();

        public DashboardViewModel()
        {
            var context = new HawalatyDbContext();
            _authService = new AuthenticationService(context);
            _transferService = new TransferService(context, _authService);
            _localization = LocalizationManager.Instance;
            _reportService = new ReportService();

            DailyReportCommand = new RelayCommand(ExecuteDailyReport);
            WeeklyReportCommand = new RelayCommand(ExecuteWeeklyReport);
            MonthlyReportCommand = new RelayCommand(ExecuteMonthlyReport);

            // Subscribe to language changes
            _localization.LanguageChanged += OnLanguageChanged;

            LoadDataAsync();
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        #region Properties

        public ObservableCollection<Transfer> RecentTransfers
        {
            get => _recentTransfers;
            set
            {
                _recentTransfers = value;
                OnPropertyChanged();
            }
        }

        public TransferStatistics Statistics
        {
            get => _statistics;
            set
            {
                _statistics = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(TotalIncomingAmount));
                OnPropertyChanged(nameof(TotalOutgoingAmount));
                OnPropertyChanged(nameof(TotalCommissionAmount));
                OnPropertyChanged(nameof(PendingCount));
                OnPropertyChanged(nameof(CompletedCount));
                OnPropertyChanged(nameof(CancelledCount));
            }
        }

        // Localized Properties
        public string PageTitle => _localization.GetString("Dashboard");
        public string StatisticsTitle => "Statistics Overview";
        public string TotalIncomingText => _localization.GetString("TotalIncoming");
        public string TotalOutgoingText => _localization.GetString("TotalOutgoing");
        public string TotalCommissionText => _localization.GetString("TotalCommission");
        public string PendingTransfersText => _localization.GetString("PendingTransfers");
        public string CompletedTransfersText => _localization.GetString("CompletedTransfers");
        public string CancelledTransfersText => "Cancelled Transfers";
        public string QuickReportsText => "Quick Reports";
        public string DailyReportText => _localization.GetString("DailyReport");
        public string WeeklyReportText => _localization.GetString("WeeklyReport");
        public string MonthlyReportText => _localization.GetString("MonthlyReport");
        public string RecentTransfersText => "Recent Transfers";

        // Statistics Display Properties
        public string TotalIncomingAmount => $"{Statistics.TotalIncomingTRY:F2}";
        public string TotalOutgoingAmount => $"{Statistics.TotalOutgoingLYD:F2}";
        public string TotalCommissionAmount => $"{Statistics.TotalCommission:F2}";
        public string PendingCount => Statistics.TotalPending.ToString();
        public string CompletedCount => Statistics.TotalCompleted.ToString();
        public string CancelledCount => Statistics.TotalCancelled.ToString();

        // Column Headers for DataGrid
        public string TransferIdText => _localization.GetString("TransferId");
        public string SenderNameText => _localization.GetString("SenderName");
        public string ReceiverNameText => _localization.GetString("ReceiverName");
        public string AmountText => _localization.GetString("Amount");
        public string CurrencyText => _localization.GetString("Currency");
        public string StatusText => _localization.GetString("Status");
        public string CreatedDateText => "Created Date";

        #endregion

        #region Commands

        public ICommand DailyReportCommand { get; }
        public ICommand WeeklyReportCommand { get; }
        public ICommand MonthlyReportCommand { get; }

        private async void ExecuteDailyReport(object? parameter)
        {
            var today = DateTime.Today;
            await GenerateReport(today, today, "Daily");
        }

        private async void ExecuteWeeklyReport(object? parameter)
        {
            var today = DateTime.Today;
            var weekStart = today.AddDays(-(int)today.DayOfWeek);
            await GenerateReport(weekStart, today, "Weekly");
        }

        private async void ExecuteMonthlyReport(object? parameter)
        {
            var today = DateTime.Today;
            var monthStart = new DateTime(today.Year, today.Month, 1);
            await GenerateReport(monthStart, today, "Monthly");
        }

        private async Task GenerateReport(DateTime fromDate, DateTime toDate, string reportType)
        {
            try
            {
                var transfers = await _transferService.GetTransfersAsync(fromDate, toDate);
                var fileName = $"{reportType}_Report_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";
                var filePath = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), fileName);
                
                await _reportService.GenerateTransfersReportExcelAsync(transfers, filePath, fromDate, toDate);
                
                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(filePath) { UseShellExecute = true });
            }
            catch (Exception ex)
            {
                // TODO: Show error message to user
                System.Windows.MessageBox.Show($"Error generating report: {ex.Message}", "Error", 
                    System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            }
        }

        #endregion

        private async void LoadDataAsync()
        {
            try
            {
                // Load statistics
                Statistics = await _transferService.GetStatisticsAsync();

                // Load recent transfers (last 10)
                var transfers = await _transferService.GetTransfersAsync();
                RecentTransfers.Clear();
                
                var recentCount = Math.Min(10, transfers.Count);
                for (int i = 0; i < recentCount; i++)
                {
                    RecentTransfers.Add(transfers[i]);
                }
            }
            catch (Exception ex)
            {
                // TODO: Handle error appropriately
                System.Diagnostics.Debug.WriteLine($"Error loading dashboard data: {ex.Message}");
            }
        }

        private void OnLanguageChanged(object? sender, EventArgs e)
        {
            // Notify all localized properties to update
            OnPropertyChanged(nameof(PageTitle));
            OnPropertyChanged(nameof(TotalIncomingText));
            OnPropertyChanged(nameof(TotalOutgoingText));
            OnPropertyChanged(nameof(TotalCommissionText));
            OnPropertyChanged(nameof(PendingTransfersText));
            OnPropertyChanged(nameof(CompletedTransfersText));
            OnPropertyChanged(nameof(DailyReportText));
            OnPropertyChanged(nameof(WeeklyReportText));
            OnPropertyChanged(nameof(MonthlyReportText));
            OnPropertyChanged(nameof(TransferIdText));
            OnPropertyChanged(nameof(SenderNameText));
            OnPropertyChanged(nameof(ReceiverNameText));
            OnPropertyChanged(nameof(AmountText));
            OnPropertyChanged(nameof(CurrencyText));
            OnPropertyChanged(nameof(StatusText));
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}