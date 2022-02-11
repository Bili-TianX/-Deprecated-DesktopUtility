using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Media;
using System.Windows.Threading;

namespace DesktopUtility
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : System.Windows.Application
    {
        private Thread? thread;

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
                        Window? window = new()
                        {
                            Width = 400,
                            Height = 300,
                            Topmost = true
                        };

                        IEnumerable<Data.TaskData>? result1 = from item in Data.TaskFactory.list
                                                              where !item.check
                                                              select item;
                        IEnumerable<Data.PlanData>? result2 = from item in Data.PlanFactory.plans
                                                              where !item.check && item.begin <= now && now <= item.end
                                                              select item;

                        StringBuilder builder = new();
                        foreach (Data.TaskData? a in result1)
                        {
                            builder.Append(a.content).Append('\n');
                        }
                        foreach (Data.PlanData? a in result2)
                        {
                            builder.Append(a.title).Append('\n');
                        }

                        if (result1.Any() || result2.Any()) // WA
                        {
                            //var player = new SoundPlayer(DesktopUtility.Resources.Resource1.wa);
                            //Dispatcher.BeginInvoke(() => player.Play());

                            Grid grid = new();
                            grid.RowDefinitions.Add(new RowDefinition()
                            {
                                Height = new GridLength(48, GridUnitType.Pixel)
                            });
                            grid.RowDefinitions.Add(new RowDefinition());
                            grid.ColumnDefinitions.Add(new ColumnDefinition());
                            TextBlock? detail = new()
                            {
                                Text = builder.ToString(),
                                VerticalAlignment = VerticalAlignment.Center,
                                HorizontalAlignment = System.Windows.HorizontalAlignment.Center,
                                FontSize = 16,
                            };
                            Grid.SetRow(detail, 1);
                            Grid.SetColumn(detail, 0);

                            grid.Children.Add(detail);
                            TextBlock textBlock = new()
                            {
                                Text = "您还有未完成的任务：",
                                VerticalAlignment = VerticalAlignment.Center,
                                HorizontalAlignment = System.Windows.HorizontalAlignment.Center,
                                FontSize = 24,
                                Foreground = new SolidColorBrush(Util.ColorUtil.Red)
                            };
                            Grid.SetRow(textBlock, 0);
                            Grid.SetColumn(textBlock, 0);
                            grid.Children.Add(textBlock);

                            window.Content = grid;

                        }
                        else // AC
                        {
                            //var player = new SoundPlayer(DesktopUtility.Resources.Resource1.ac);
                            //Dispatcher.BeginInvoke(() => player.Play());

                            Grid grid = new();
                            grid.RowDefinitions.Add(new RowDefinition()
                            {
                                Height = new GridLength(48, GridUnitType.Pixel)
                            });
                            grid.RowDefinitions.Add(new RowDefinition());
                            grid.ColumnDefinitions.Add(new ColumnDefinition());
                            Image? img = new()
                            {
                                Source = Util.ImageUtil.ToImageSource(DesktopUtility.Resources.Resource1.yes)
                            };
                            Grid.SetRow(img, 1);
                            Grid.SetColumn(img, 0);
                            grid.Children.Add(img);
                            TextBlock textBlock = new()
                            {
                                Text = "恭喜你完成了今天的所有任务！",
                                VerticalAlignment = VerticalAlignment.Center,
                                HorizontalAlignment = System.Windows.HorizontalAlignment.Center,
                                FontSize = 24
                            };
                            Grid.SetRow(textBlock, 0);
                            Grid.SetColumn(textBlock, 0);
                            grid.Children.Add(textBlock);

                            window.Content = grid;
                        }



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
            Data.Setting.LoadSetting();
            Data.Love.LoadFromFile();

            thread = new(f);
            thread.IsBackground = true;
            thread.Start();
        }

        private void Application_Exit(object sender, ExitEventArgs e)
        {
            Data.Setting.SaveSetting();
            Data.IconFactory.SaveToFile();
            Data.PlanFactory.SaveToFile();
            Data.DayFactory.SaveToFile();
            Data.TaskFactory.SaveToFile();
            Data.Love.SaveToFile();

            GC.Collect();
        }
    }
}
