using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Threading;

namespace DesktopUtility
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : System.Windows.Application
    {
        private Thread thread;

        public void f()
        {
            HashSet<(int year, int month, int day)> set = new();
            while (true)
            {
                DateTime now = DateTime.Now;
                (int Year, int Month, int Day) tmp = (now.Year, now.Month, now.Day);
                if (!set.Contains(tmp) && now.Hour == 22 && now.Minute == 0)
                {
                    set.Add(tmp);
                    Dispatcher.BeginInvoke(() =>
                    {
                        Window? window = new Window()
                        {
                            Width = 300,
                            Height = 300
                        };
                        window.Left = SystemInformation.WorkingArea.Size.Width - window.Width;
                        window.Top = SystemInformation.WorkingArea.Size.Height - window.Height;
                        window.Show();
                    });
                }
                Thread.Sleep(1000);
            }
        }

        private void Application_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            System.Windows.MessageBox.Show("抱歉，程序出现了异常，请把此窗口截图发给我\n错误信息：" + e.Exception.ToString(), e.Exception.GetType().ToString());
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            Data.PlanFactory.LoadFromFile();
            Data.DayFactory.LoadFromFile();
            Data.TaskFactory.LoadFromFile();
            thread = new(f);
            thread.IsBackground = true;
            thread.Start();
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
