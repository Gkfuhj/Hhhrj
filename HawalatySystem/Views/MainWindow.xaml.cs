using System.Windows;
using System.Windows.Controls;
using HawalatySystem.ViewModels;
using HawalatySystem.Resources.Localization;

namespace HawalatySystem.Views
{
    public partial class MainWindow : Window
    {
        private readonly MainWindowViewModel _viewModel;

        public MainWindow()
        {
            InitializeComponent();
            _viewModel = new MainWindowViewModel();
            DataContext = _viewModel;
            
            // Load dashboard by default
            LoadDashboard();
        }

        private void NavigationButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is string tag)
            {
                switch (tag)
                {
                    case "Dashboard":
                        LoadDashboard();
                        break;
                    case "Transfers":
                        LoadTransfers();
                        break;
                    case "NewTransfer":
                        LoadNewTransfer();
                        break;
                    case "Agents":
                        LoadAgents();
                        break;
                    case "Reports":
                        LoadReports();
                        break;
                    case "ExchangeRates":
                        LoadExchangeRates();
                        break;
                    case "Users":
                        LoadUsers();
                        break;
                    case "Settings":
                        LoadSettings();
                        break;
                }
            }
        }

        private void LanguageComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (LanguageComboBox.SelectedIndex == 0)
            {
                LocalizationManager.Instance.CurrentLanguage = "en";
                FlowDirection = FlowDirection.LeftToRight;
            }
            else if (LanguageComboBox.SelectedIndex == 1)
            {
                LocalizationManager.Instance.CurrentLanguage = "ar";
                FlowDirection = FlowDirection.RightToLeft;
            }
        }

        private void LoadDashboard()
        {
            var dashboardView = new DashboardView();
            ContentArea.Content = dashboardView;
        }

        private void LoadTransfers()
        {
            // TODO: Create TransfersView
            var placeholder = new TextBlock { Text = "Transfers View - Coming Soon", FontSize = 24, HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Center };
            ContentArea.Content = placeholder;
        }

        private void LoadNewTransfer()
        {
            // TODO: Create NewTransferView
            var placeholder = new TextBlock { Text = "New Transfer View - Coming Soon", FontSize = 24, HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Center };
            ContentArea.Content = placeholder;
        }

        private void LoadAgents()
        {
            // TODO: Create AgentsView
            var placeholder = new TextBlock { Text = "Agents View - Coming Soon", FontSize = 24, HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Center };
            ContentArea.Content = placeholder;
        }

        private void LoadReports()
        {
            // TODO: Create ReportsView
            var placeholder = new TextBlock { Text = "Reports View - Coming Soon", FontSize = 24, HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Center };
            ContentArea.Content = placeholder;
        }

        private void LoadExchangeRates()
        {
            // TODO: Create ExchangeRatesView
            var placeholder = new TextBlock { Text = "Exchange Rates View - Coming Soon", FontSize = 24, HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Center };
            ContentArea.Content = placeholder;
        }

        private void LoadUsers()
        {
            // TODO: Create UsersView
            var placeholder = new TextBlock { Text = "Users View - Coming Soon", FontSize = 24, HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Center };
            ContentArea.Content = placeholder;
        }

        private void LoadSettings()
        {
            // TODO: Create SettingsView
            var placeholder = new TextBlock { Text = "Settings View - Coming Soon", FontSize = 24, HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Center };
            ContentArea.Content = placeholder;
        }
    }
}