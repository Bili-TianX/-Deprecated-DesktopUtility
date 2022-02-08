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
            System.Windows.MessageBox.Show("抱歉，程序出现了异常，请把此窗口截图发给我\n错误信息：" + e.Exception.ToString(), e.Exception.GetType().ToString());
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            Data.PlanFactory.LoadFromFile();
            Data.DayFactory.LoadFromFile();
            Data.TaskFactory.LoadFromFile();
        }

        private void Application_Exit(object sender, ExitEventArgs e)
        {
            Data.IconFactory.SaveToFile();
            Data.PlanFactory.SaveToFile();
            Data.DayFactory.SaveToFile();
            Data.TaskFactory.SaveToFile();
            GC.Collect();
        }
    }
}
