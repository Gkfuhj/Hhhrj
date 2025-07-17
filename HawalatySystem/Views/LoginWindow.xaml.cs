using System.Windows;
using HawalatySystem.ViewModels;

namespace HawalatySystem.Views
{
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
            DataContext = new LoginViewModel();
        }
    }
}