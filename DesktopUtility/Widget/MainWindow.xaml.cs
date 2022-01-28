using DesktopUtility.Util;
using DesktopUtility.Widget;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;

namespace DesktopUtility
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public unsafe partial class MainWindow : Window
    {
        public const int COLUMN_COUNT = 1;
        public const bool onBottom = false;
        public static Style? boxItemStyle;

        private static IntPtr SearchDesktopHandle()
        {
            IntPtr hRoot = Util.WinAPI.GetDesktopWindow();
            IntPtr hDesktop = Util.WinAPI.FindWindowEx(hRoot, IntPtr.Zero, "WorkerW", string.Empty);
            while (true)
            {
                IntPtr hShellDll = Util.WinAPI.FindWindowEx(hDesktop, IntPtr.Zero, "SHELLDLL_DefView", string.Empty);
                if (hShellDll != IntPtr.Zero)
                {
                    return hDesktop;
                }
                hDesktop = Util.WinAPI.FindWindowEx(hRoot, hDesktop, "WorkerW", string.Empty);
            }
        }

        private void MainWindow_onLoaded(object sender, EventArgs e)
        {
            if (onBottom)
            {
                const int GWL_STYLE = (-16);
                const ulong WS_CHILD = 0x40000000;
                IntPtr hWnd = new WindowInteropHelper(this).Handle;
                ulong iWindowStyle = Util.WinAPI.GetWindowLong(hWnd, GWL_STYLE);
#pragma warning disable CA1806 // 不要忽略方法结果
                WinAPI.SetWindowLong(hWnd, GWL_STYLE, (iWindowStyle | WS_CHILD));
#pragma warning restore CA1806 // 不要忽略方法结果

                IntPtr desktopHandle = SearchDesktopHandle();
                Util.WinAPI.SetParent(hWnd, desktopHandle);
            }
        }

        public unsafe MainWindow()
        {
            InitializeComponent();

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

            ReLayout();
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
            AppIcon? icon = new AppIcon(data);
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
    }
}
