using System.Configuration;
using System.Data;
using System.Windows;
using TradeOps.Helper;

namespace TradeOps
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            DB_Queries.InitializeDatabase();
            this.ShutdownMode = ShutdownMode.OnExplicitShutdown;
        }
    }

}
