using System.Windows;
using Microsoft.EntityFrameworkCore;
using HawalatySystem.Data;
using HawalatySystem.Services;

namespace HawalatySystem
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Initialize database
            using (var context = new HawalatyDbContext())
            {
                context.Database.EnsureCreated();
            }
        }
    }
}