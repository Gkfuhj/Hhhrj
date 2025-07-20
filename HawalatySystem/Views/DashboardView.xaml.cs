using System.Windows.Controls;
using HawalatySystem.ViewModels;

namespace HawalatySystem.Views
{
    public partial class DashboardView : UserControl
    {
        public DashboardView()
        {
            InitializeComponent();
            DataContext = new DashboardViewModel();
        }
    }
}