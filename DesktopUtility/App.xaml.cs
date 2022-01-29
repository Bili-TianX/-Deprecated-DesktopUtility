using Hardcodet.Wpf.TaskbarNotification;
using System;
using System.Windows;
using System.Windows.Forms;

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
            Data.IconFactory.LoadFromFile();
        }

        private void Application_Exit(object sender, ExitEventArgs e)
        {
            Data.IconFactory.SaveToFile();
            GC.Collect();
        }
    }
}
