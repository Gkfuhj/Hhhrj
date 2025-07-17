using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using HawalatySystem.Data;
using HawalatySystem.Services;
using HawalatySystem.Resources.Localization;
using HawalatySystem.Views;

namespace HawalatySystem.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private readonly AuthenticationService _authService;
        private readonly LocalizationManager _localization;

        public MainWindowViewModel()
        {
            _localization = LocalizationManager.Instance;
            _authService = new AuthenticationService(new HawalatyDbContext());
            
            LogoutCommand = new RelayCommand(ExecuteLogout);
            
            // Subscribe to language changes
            _localization.LanguageChanged += OnLanguageChanged;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        #region Properties

        public string Title => _localization.GetString("SystemTitle");
        public string CurrentUserName => _authService.CurrentUser?.FullName ?? "User";
        public string LogoutText => _localization.GetString("Logout");
        public bool IsAdminVisible => _authService.CurrentUser?.Role == Models.UserRole.Admin;

        // Navigation texts
        public string DashboardText => _localization.GetString("Dashboard");
        public string TransfersText => _localization.GetString("Transfers");
        public string NewTransferText => _localization.GetString("NewTransfer");
        public string AgentsText => _localization.GetString("Agents");
        public string ReportsText => _localization.GetString("Reports");
        public string ExchangeRatesText => _localization.GetString("ExchangeRates");
        public string UsersText => _localization.GetString("Users");
        public string SettingsText => _localization.GetString("Settings");

        #endregion

        #region Commands

        public ICommand LogoutCommand { get; }

        private void ExecuteLogout(object? parameter)
        {
            _authService.Logout();
            
            var loginWindow = new LoginWindow();
            loginWindow.Show();
            
            Application.Current.MainWindow?.Close();
            Application.Current.MainWindow = loginWindow;
        }

        #endregion

        private void OnLanguageChanged(object? sender, EventArgs e)
        {
            // Notify all localized properties to update
            OnPropertyChanged(nameof(Title));
            OnPropertyChanged(nameof(LogoutText));
            OnPropertyChanged(nameof(DashboardText));
            OnPropertyChanged(nameof(TransfersText));
            OnPropertyChanged(nameof(NewTransferText));
            OnPropertyChanged(nameof(AgentsText));
            OnPropertyChanged(nameof(ReportsText));
            OnPropertyChanged(nameof(ExchangeRatesText));
            OnPropertyChanged(nameof(UsersText));
            OnPropertyChanged(nameof(SettingsText));
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}