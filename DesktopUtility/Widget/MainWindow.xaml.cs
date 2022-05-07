using DesktopUtility.Util;
using DesktopUtility.Widget;
using Hardcodet.Wpf.TaskbarNotification;
using System;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Interop;
using System.Windows.Media;

namespace DesktopUtility
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public unsafe partial class MainWindow : Window
    {
        public const bool onBottom = true;
        private static Style? boxItemStyle;
        public TaskbarIcon taskbarIcon;
        public ContextMenu iconMenu;
        private Thread thread;

        private static IntPtr SearchDesktopHandle()
        {
            IntPtr desktop = Util.WinAPI.GetDesktopWindow();
            IntPtr hWorkerW = IntPtr.Zero;
            IntPtr hShellViewWin;
            do
            {
                hWorkerW = Util.WinAPI.FindWindowEx(desktop, hWorkerW, "WorkerW", string.Empty);
                hShellViewWin = Util.WinAPI.FindWindowEx(hWorkerW, IntPtr.Zero, "SHELLDLL_DefView", string.Empty);
            } while (hShellViewWin == IntPtr.Zero && hWorkerW != IntPtr.Zero);
            return hWorkerW;
        }

        ~MainWindow()
        {
            taskbarIcon.Visibility = Visibility.Hidden;
            taskbarIcon.Dispose();
        }

        public void OnLove(object sender, EventArgs e)
        {
            Window w = new()
            {
                Width = 400,
                Height = 400,
                WindowStartupLocation = WindowStartupLocation.CenterScreen,
            };
            Grid g = new();
            g.RowDefinitions.Add(new RowDefinition());
            g.ColumnDefinitions.Add(new ColumnDefinition());
            TextBlock t = new()
            {
                Text = Data.Love.Get(),
                FontSize = 36,
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Center,
                TextWrapping = TextWrapping.Wrap
            };
            g.Children.Add(t);
            Grid.SetRow(t, 0);
            Grid.SetColumn(t, 0);
            w.Content = g;

            w.ShowDialog();
        }

        public void OnSetting(object sender, EventArgs e)
        {
            SettingWindow w = new();
            w.ShowDialog();
        }

        private void MainWindow_onLoaded(object sender, EventArgs e)
        {
            if (onBottom)
            {
#pragma warning disable CS0162 // 检测到无法访问的代码
                const int GWL_STYLE = (-16);
#pragma warning restore CS0162 // 检测到无法访问的代码
                const ulong WS_CHILD = 0x40000000;
                IntPtr hWnd = new WindowInteropHelper(this).Handle;
                ulong iWindowStyle = Util.WinAPI.GetWindowLong(hWnd, GWL_STYLE);
#pragma warning disable CA1806 // 不要忽略方法结果
                WinAPI.SetWindowLong(hWnd, GWL_STYLE, (iWindowStyle | WS_CHILD));
#pragma warning restore CA1806 // 不要忽略方法结果

                IntPtr desktopHandle = SearchDesktopHandle();
                Util.WinAPI.SetParent(hWnd, desktopHandle);
            }

            Left = Top = 0;
            Width = Screen.AllScreens[0].WorkingArea.Width;
            Height = Screen.AllScreens[0].WorkingArea.Height;



            /*MenuItem? a = new()
            {
                Icon = Util.ImageUtil.ToImage(DesktopUtility.Resources.MyRes.heart),
                Header = "Love"
            };
            MyMenu.Items.Add(a);
            a.Click += OnLove;*/

            MenuItem? b = new()
            {
                Icon = Util.ImageUtil.ToImage(DesktopUtility.Resources.MyRes.set),
                Header = "设置"
            };
            MyMenu.Items.Add(b);
            b.Click += OnSetting;
            thread = new Thread(run)
            {
                IsBackground = true
            };
            thread.Start();
        }

        public void run()
        {
            while (true)
            {
                DateTime now = DateTime.Now;
                System.Collections.Generic.IEnumerable<Data.PlanData>? result = from item in Data.PlanFactory.plans
                                                                                where item.begin <= now && now <= item.end && !item.check
                                                                                select item;

                var r = from item in calendar.list
                             where item.Time.Month <= now.Month && item.Time.Day < now.Day && item.Time.Year <= now.Year
                             select item;
                

                Dispatcher.BeginInvoke(() =>
                {
                    foreach (var item in r)
                    {
                        item.Visibility = Visibility.Hidden;
                        item.IsEnabled = false;
                    }

                    TipList.Items.Clear();
                    foreach (Data.PlanData? item in result)
                    {
                        TipList.Items.Add(new ListBoxItem()
                        {
                            Content = item.title,
                            Style = boxItemStyle
                        });
                    }

                });
                Thread.Sleep(10 * 1000);
            }
        }

        public void AddDay(Data.DayData data)
        {
            DateTime time = data.time;
            DateTime now = DateTime.Now;
            if (time.Month <= now.Month && time.Day < now.Day)
            {
                now = new DateTime(now.Year - 1, now.Month, now.Day);
            }

            DayList.Items.Add(new ListBoxItem()
            {
                Style = boxItemStyle,
                Content = $"{data.name}({data.time:M})({(int)(time - now).TotalDays}天)"
            });
        }

        public unsafe MainWindow()
        {
            InitializeComponent();


            iconMenu = new ContextMenu();
            updateBackground();

            System.Collections.ICollection? values = App.Current.MainWindow.Resources.Values;

            foreach (object? value in values)
            {
                if (value.GetType() == typeof(Style) && ((Style)value).TargetType.Name == nameof(ListBoxItem))
                {
                    boxItemStyle = (Style)value;
                    break;
                }
            }

            for (int i = 0; i < Data.Setting.Column_Count; i++)
            {
                LaunchPad.ColumnDefinitions.Add(new ColumnDefinition()
                {
                    Width = new GridLength(1, GridUnitType.Star)
                });
            }

            Icon = Util.ImageUtil.ToImageSource(DesktopUtility.Resources.Resource1.icon);
            addAppItem.Icon = Util.ImageUtil.ToImage(DesktopUtility.Resources.Resource1.addIcon);
            addPlanItem.Icon = Util.ImageUtil.ToImage(DesktopUtility.Resources.Resource1.addIcon);
            addDayItem.Icon = Util.ImageUtil.ToImage(DesktopUtility.Resources.Resource1.addIcon);
            addTaskItem.Icon = Util.ImageUtil.ToImage(DesktopUtility.Resources.Resource1.addIcon);
            showPlanItem.Icon = Util.ImageUtil.ToImage(DesktopUtility.Resources.Resource1.showIcon);
            editTimeItem.Icon = Util.ImageUtil.ToImage(DesktopUtility.Resources.Resource1.renameIcon);
            copyrightItem.Icon = Util.ImageUtil.ToImage(DesktopUtility.Resources.Resource1.copyright);
            //loveItem.Icon = Util.ImageUtil.ToImage(DesktopUtility.Resources.Resource1.heart);
            //settingMenuItem.Icon = Util.ImageUtil.ToImage(DesktopUtility.Resources.Resource1.setting);


            Data.IconFactory.LoadFromFile();
            ReLayout();
            foreach (Data.DayData? data in Data.DayFactory.list)
            {
                AddDay(data);
            }

            foreach (Data.TaskData? data in Data.TaskFactory.list)
            {
                AddTask(data);
            }

            taskbarIcon = new TaskbarIcon()
            {
                Visibility = Visibility.Visible,
                Icon = DesktopUtility.Resources.Resource1.exeIcon,
                ToolTipText = "DesktopUtility",
                ContextMenu = iconMenu,
                MenuActivation = PopupActivationMode.RightClick
            };

            MenuItem? item = new()
            {
                Header = "退出"
            };
            item.Click += (o, e) => { System.Windows.Application.Current.Shutdown(0); };
            MenuItem? item2 = new()
            {
                Header = "显示/隐藏"
            };
            item2.Click += (o, e) =>
            {
                if (Visibility == Visibility.Visible)
                {
                    Hide();
                }
                else
                {
                    Show();
                }
            };
            iconMenu.Items.Add(item2);
            iconMenu.Items.Add(item);
        }

        public void AttachPlan()
        {
            foreach (DateLabel? item in calendar.list)
            {
                item.RemovePlan();
            }

            foreach (Data.PlanData plan in Data.PlanFactory.plans)
            {
                AttachPlan(plan);
            }
        }

        public void AttachPlan(Data.PlanData plan)
        {
            DateTime begin = new(plan.begin.Year, plan.begin.Month, plan.begin.Day);
            DateTime end = new(plan.end.Year, plan.end.Month, plan.end.Day);
            foreach (DateLabel? item in calendar.list)
            {
                if (item != null)
                {
                    if (begin <= item.Time && item.Time <= end)
                    {
                        item.AttachPlan();
                    }
                }
            }
        }

        public void ReLayout()
        {
            LaunchPad.Children.Clear();
            LaunchPad.RowDefinitions.Clear();
            LaunchPad.ColumnDefinitions.Clear();
            for (int i = 0; i < Data.Setting.Column_Count; ++i)
            {
                LaunchPad.ColumnDefinitions.Add(new ColumnDefinition());
            }

            for (int i = 0; i < Data.IconFactory.icons.Count; ++i)
            {
                AddIcon(Data.IconFactory.icons[i], i);
            }
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog ofd = new()
            {
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.CommonPrograms),
                Filter = "(*.exe)|*.exe",
                Multiselect = false
            };
            bool? r = ofd.ShowDialog(this);
            if (Data.IconFactory.ExistByPath(ofd.FileName))
            {
                System.Windows.MessageBox.Show("应用已存在: " + ofd.FileName, "错误");
                return;
            }

            if (r != null && (bool)r)
            {
                IconNameDialog? dialog = new(ofd.SafeFileName[..^4].Capitalize());
                dialog.ShowDialog();
                if (!dialog.ok)
                {
                    return;
                }

                string? name = dialog.IconName;

                AddIcon(new Data.IconData(name, ofd.FileName));
            }
        }

        private void AddIcon(Data.IconData data)
        {
            AppIcon? icon = new(data);
            Data.IconFactory.icons.Add(icon);
            AddIcon(icon);
        }

        public void updateBackground()
        {
            if (string.IsNullOrWhiteSpace(Data.Setting.Background_Image))
            {
                Background =
                    new System.Windows.Media.ImageBrush(
                        Util.ImageUtil.ToImageSource(DesktopUtility.Resources.Resource1.bg2));
            }
            else
            {
                Background = new ImageBrush()
                {
                    ImageSource = (ImageSource)new ImageSourceConverter().ConvertFrom(Data.Setting.Background_Image)!
                };
            }
        }

        private void AddIcon(AppIcon icon, int index = -1)
        {
            if (index == -1)
            {
                index = Data.IconFactory.icons.Count - 1;
            }

            LaunchPad.Children.Add(icon);

            if (LaunchPad.RowDefinitions.Count <= index / Data.Setting.Column_Count)
            {
                LaunchPad.RowDefinitions.Add(new RowDefinition());
            }

            Grid.SetColumn(icon, index % Data.Setting.Column_Count);
            Grid.SetRow(icon, index / Data.Setting.Column_Count);

            icon.SetImage();
        }

        private void addPlanItem_Click(object sender, RoutedEventArgs e)
        {
            PlanDialog? dialog = new();
            dialog.ShowDialog();
            if (dialog.ok)
            {
                Data.PlanData plan = dialog.Data;
                Data.PlanFactory.plans.Add(plan);
                AttachPlan(plan);
            }
        }

        public void ShowPlan(DateTime? time = null)
        {
            if (time != null)
            {
                new PlanWindow((DateTime)time).ShowDialog();
            }
            else
            {
                new PlanWindow().ShowDialog();
            }
        }

        private void showPlanItem_Click(object sender, RoutedEventArgs e)
        {
            ShowPlan();
        }

        private void editTimeItem_Click(object sender, RoutedEventArgs e)
        {
            EditTimeDialog? dialog = new(calendar.year, calendar.month);
            dialog.ShowDialog();
            if (dialog.ok)
            {
                int year = dialog.Year;
                int month = dialog.Month;

                if (year != calendar.year || month != calendar.month)
                {
                    //TODO: change
                    calendar.SetTime(year, month);
                    AttachPlan();
                }
            }
        }

        private void addDayItem_Click(object sender, RoutedEventArgs e)
        {
            DayDialog? dialog = new();
            dialog.ShowDialog();
            if (dialog.ok)
            {
                Data.DayData? tmp = dialog.Data;
                Data.DayFactory.list.Add(tmp);
                AddDay(tmp);
            }
        }

        public void AddTask(Data.TaskData data)
        {
            WrapPanel panel = new();
            System.Windows.Controls.CheckBox box = new()
            {
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Center,
                LayoutTransform = new ScaleTransform()
                {
                    ScaleX = 1.5,
                    ScaleY = 1.5,
                },
                IsChecked = data.check
            };
            TextBlock block = new() { Text = data.content };
            box.Click += (o, e) =>
            {
#pragma warning disable CS8602 // 解引用可能出现空引用。
                Data.TaskFactory.list.Find((item) => item.content == block.Text).check = (bool)box.IsChecked;
#pragma warning restore CS8602 // 解引用可能出现空引用。
            };

            panel.Children.Add(box);
            panel.Children.Add(block);

            TaskList.Items.Add(new ListBoxItem()
            {
                Style = boxItemStyle,
                Content = panel
            });
        }

        private void addTaskItem_Click(object sender, RoutedEventArgs e)
        {
            Widget.TaskDialog? dialog = new();
            dialog.ShowDialog();
            if (dialog.ok)
            {
                Data.TaskData? tmp = dialog.Data;
                Data.TaskFactory.list.Add(tmp);
                AddTask(tmp);
            }
        }

        private void DayList_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            ListBoxItem? item = (ListBoxItem?)DayList.SelectedItem;
            if (item != null)
            {
                if (System.Windows.MessageBox.Show($"删除重要日 {item?.Content} ?", "删除", MessageBoxButton.YesNo) ==
                    MessageBoxResult.Yes)
                {
                    DayList.Items.Remove(item);
#pragma warning disable CS8600 // 将 null 字面量或可能为 null 的值转换为非 null 类型。
                    string name = (string)item?.Content;
                    Data.DayFactory.list.RemoveAll((item) => name?.Split('(')[0] == item.name);
#pragma warning restore CS8600 // 将 null 字面量或可能为 null 的值转换为非 null 类型。
                }
            }
        }

        private void TaskList_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            ListBoxItem? item = (ListBoxItem?)TaskList.SelectedItem;
            if (item != null)
            {
                WrapPanel? tmp = (WrapPanel)item.Content;
                string? content = ((TextBlock)tmp.Children[1]).Text;

                if (System.Windows.MessageBox.Show($"删除每日需做 {content} ?", "删除", MessageBoxButton.YesNo) ==
                    MessageBoxResult.Yes)
                {
                    TaskList.Items.Remove(item);
                    Data.TaskFactory.list.RemoveAll((item) => content == item.content);
                }
            }
        }

        private void copyrightItem_Click(object sender, RoutedEventArgs e)
        {
            new Copyright().ShowDialog();
        }
    }
}