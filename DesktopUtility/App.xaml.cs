using System;
using System.Windows;

namespace DesktopUtility
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : System.Windows.Application
    {


        private void Application_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            System.Windows.MessageBox.Show(e.Exception.ToString(), e.Exception.GetType().ToString());
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            Data.PlanFactory.LoadFromFile();
        }

        private void Application_Exit(object sender, ExitEventArgs e)
        {
            Data.IconFactory.SaveToFile();
            Data.PlanFactory.SaveToFile();
            GC.Collect();
        }
    }
}
