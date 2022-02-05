﻿using DesktopUtility.Util;
using DesktopUtility.Widget;
using Hardcodet.Wpf.TaskbarNotification;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Interop;

namespace DesktopUtility
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public unsafe partial class MainWindow : Window
    {
        public const int COLUMN_COUNT = 4;
        public const bool onBottom = true;
        public static Style? boxItemStyle;
        public TaskbarIcon taskbarIcon;
        public ContextMenu iconMenu;

        private static IntPtr SearchDesktopHandle()
        {
            IntPtr desktop = Util.WinAPI.GetDesktopWindow();
            IntPtr hWorkerW = IntPtr.Zero;
            IntPtr hShellViewWin = IntPtr.Zero;
            do
            {
                hWorkerW = Util.WinAPI.FindWindowEx(desktop, hWorkerW, "WorkerW", string.Empty);
                hShellViewWin = Util.WinAPI.FindWindowEx(hWorkerW, IntPtr.Zero, "SHELLDLL_DefView", string.Empty);
            } while (hShellViewWin == IntPtr.Zero && hWorkerW != IntPtr.Zero);
            return hShellViewWin;
        }

        ~MainWindow()
        {
            taskbarIcon.Visibility = Visibility.Hidden;
            taskbarIcon.Dispose();
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
            Width = SystemInformation.WorkingArea.Size.Width;
            Height = SystemInformation.WorkingArea.Size.Height;
            Left = Top = 0;
        }

        public unsafe MainWindow()
        {
            InitializeComponent();
            iconMenu = new ContextMenu();
            Background = new System.Windows.Media.ImageBrush(Util.ImageUtil.ToImageSource(DesktopUtility.Resources.Resource1.bg2));
            System.Collections.ICollection? values = App.Current.MainWindow.Resources.Values;

            foreach (object? value in values)
            {
                if (value.GetType() == typeof(Style) && ((Style)value).TargetType.Name == nameof(ListBoxItem))
                {
                    boxItemStyle = (Style)value;
                    break;
                }
            }

            for (int i = 0; i < COLUMN_COUNT; i++)
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

            Data.IconFactory.LoadFromFile();
            ReLayout();


            taskbarIcon = new TaskbarIcon()
            {
                Visibility = Visibility.Visible,
                Icon = DesktopUtility.Resources.Resource1.exeIcon,
                ToolTipText = "DesktopUtility",
                ContextMenu = iconMenu,
                MenuActivation = PopupActivationMode.RightClick
            };

            MenuItem? item = new MenuItem()
            {
                Header = "退出"
            };
            item.Click += (o, e) =>
            {
                System.Windows.Application.Current.Shutdown(0);
            };

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
            DateTime begin = new DateTime(plan.begin.Year, plan.begin.Month, plan.begin.Day);
            DateTime end = new DateTime(plan.end.Year, plan.end.Month, plan.end.Day);
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
                IconNameDialog? dialog = new IconNameDialog(ofd.SafeFileName[..^4].Capitalize());
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

        private void AddIcon(AppIcon icon, int index = -1)
        {
            if (index == -1)
            {
                index = Data.IconFactory.icons.Count - 1;
            }

            LaunchPad.Children.Add(icon);

            if (LaunchPad.RowDefinitions.Count <= index / COLUMN_COUNT)
            {
                LaunchPad.RowDefinitions.Add(new RowDefinition());
            }

            Grid.SetColumn(icon, index % COLUMN_COUNT);
            Grid.SetRow(icon, index / COLUMN_COUNT);

            icon.SetImage();
        }

        private void addPlanItem_Click(object sender, RoutedEventArgs e)
        {
            PlanDialog? dialog = new PlanDialog();
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
            EditTimeDialog? dialog = new EditTimeDialog(calendar.year, calendar.month);
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

        }

        private void addTaskItem_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
