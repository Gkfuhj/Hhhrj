using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using HawalatySystem.Data;
using HawalatySystem.Services;
using HawalatySystem.Resources.Localization;
using HawalatySystem.Views;

namespace HawalatySystem.ViewModels
{
    public class LoginViewModel : INotifyPropertyChanged
    {
        private readonly AuthenticationService _authService;
        private readonly LocalizationManager _localization;
        
        private string _username = string.Empty;
        private string _errorMessage = string.Empty;
        private bool _isLoading;
        private bool _hasError;
        private bool _isEnglish = true;
        private bool _isArabic;

        public LoginViewModel()
        {
            _localization = LocalizationManager.Instance;
            _authService = new AuthenticationService(new HawalatyDbContext());
            
            LoginCommand = new RelayCommand(ExecuteLogin, CanExecuteLogin);
            
            // Subscribe to language changes
            _localization.LanguageChanged += OnLanguageChanged;
            
            // Set default language
            IsEnglish = true;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        #region Properties

        public string Username
        {
            get => _username;
            set
            {
                _username = value;
                OnPropertyChanged();
                ((RelayCommand)LoginCommand).RaiseCanExecuteChanged();
            }
        }

        public string ErrorMessage
        {
            get => _errorMessage;
            set
            {
                _errorMessage = value;
                OnPropertyChanged();
                HasError = !string.IsNullOrEmpty(value);
            }
        }

        public bool IsLoading
        {
            get => _isLoading;
            set
            {
                _isLoading = value;
                OnPropertyChanged();
                ((RelayCommand)LoginCommand).RaiseCanExecuteChanged();
            }
        }

        public bool HasError
        {
            get => _hasError;
            set
            {
                _hasError = value;
                OnPropertyChanged();
            }
        }

        public bool IsEnglish
        {
            get => _isEnglish;
            set
            {
                _isEnglish = value;
                if (value)
                {
                    _localization.CurrentLanguage = "en";
                    _isArabic = false;
                    OnPropertyChanged(nameof(IsArabic));
                }
                OnPropertyChanged();
            }
        }

        public bool IsArabic
        {
            get => _isArabic;
            set
            {
                _isArabic = value;
                if (value)
                {
                    _localization.CurrentLanguage = "ar";
                    _isEnglish = false;
                    OnPropertyChanged(nameof(IsEnglish));
                }
                OnPropertyChanged();
            }
        }

        // Localized Properties
        public string SystemTitle => _localization.GetString("SystemTitle");
        public string WelcomeMessage => _localization.GetString("WelcomeMessage");
        public string UsernameLabel => _localization.GetString("Username");
        public string PasswordLabel => _localization.GetString("Password");
        public string LoginButtonText => _localization.GetString("Login");

        #endregion

        #region Commands

        public ICommand LoginCommand { get; }

        private bool CanExecuteLogin(object? parameter)
        {
            return !IsLoading && !string.IsNullOrWhiteSpace(Username);
        }

        private async void ExecuteLogin(object? parameter)
        {
            if (parameter is not PasswordBox passwordBox)
                return;

            var password = passwordBox.Password;
            
            if (string.IsNullOrWhiteSpace(password))
            {
                ErrorMessage = _localization.GetString("ValidationError");
                return;
            }

            IsLoading = true;
            ErrorMessage = string.Empty;

            try
            {
                var success = await _authService.LoginAsync(Username, password);
                
                if (success)
                {
                    // Open main window
                    var mainWindow = new MainWindow();
                    mainWindow.Show();
                    
                    // Close login window
                    Application.Current.MainWindow?.Close();
                    Application.Current.MainWindow = mainWindow;
                }
                else
                {
                    ErrorMessage = _localization.GetString("LoginFailed");
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Login error: {ex.Message}";
            }
            finally
            {
                IsLoading = false;
            }
        }

        #endregion

        private void OnLanguageChanged(object? sender, EventArgs e)
        {
            // Notify all localized properties to update
            OnPropertyChanged(nameof(SystemTitle));
            OnPropertyChanged(nameof(WelcomeMessage));
            OnPropertyChanged(nameof(UsernameLabel));
            OnPropertyChanged(nameof(PasswordLabel));
            OnPropertyChanged(nameof(LoginButtonText));
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    // Simple RelayCommand implementation
    public class RelayCommand : ICommand
    {
        private readonly Action<object?> _execute;
        private readonly Func<object?, bool>? _canExecute;

        public RelayCommand(Action<object?> execute, Func<object?, bool>? canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public event EventHandler? CanExecuteChanged;

        public bool CanExecute(object? parameter) => _canExecute?.Invoke(parameter) ?? true;

        public void Execute(object? parameter) => _execute(parameter);

        public void RaiseCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }
}