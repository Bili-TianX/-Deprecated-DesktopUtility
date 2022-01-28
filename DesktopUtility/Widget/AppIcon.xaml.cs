using DesktopUtility.Widget;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace DesktopUtility
{
    /// <summary>
    /// AppIcon.xaml 的交互逻辑
    /// </summary>
    public partial class AppIcon : UserControl
    {
        public  Data.IconData Data;
        public const int duration = 200;
        public Process? process = null;
        public static Color BACKGROUND = Color.FromArgb(80, 211, 211, 211);

        public AppIcon()
        {
            InitializeComponent();
            Data = new()
            {
                Path = String.Empty,
                Name = String.Empty
            };
            border.Background = new SolidColorBrush(System.Windows.Media.Color.FromArgb(0, 0, 0, 0));

            StartItem.Icon = Util.ImageUtil.ToImage(DesktopUtility.Resources.Resource1.startIcon);
            DeleteItem.Icon = Util.ImageUtil.ToImage(DesktopUtility.Resources.Resource1.deleteIcon);
            RenameItem.Icon = Util.ImageUtil.ToImage(DesktopUtility.Resources.Resource1.renameIcon);
        }

        public AppIcon(Data.IconData data) : this(data.Name, data.Path)
        {
        }

        public AppIcon(String name, String path) : this()
        {
            Data = new()
            {
                Path = path,
                Name = name
            };
             
            NameLabel.Text = name;
        }

        private void back_MouseEnter(object sender, MouseEventArgs e)
        {
            ColorAnimation animation = new()
            {
                From = Util.ColorUtil.Transparent,
                To = BACKGROUND,
                Duration = TimeSpan.FromMilliseconds(duration),
                AccelerationRatio = 0.5
            };


            border.Background.BeginAnimation(SolidColorBrush.ColorProperty, animation);
        }


        private void back_MouseLeave(object sender, MouseEventArgs e)
        {
            ColorAnimation animation = new()
            {
                To = Util.ColorUtil.Transparent,
                From = BACKGROUND,
                Duration = TimeSpan.FromMilliseconds(duration),
                AccelerationRatio = 0.5
            };


            border.Background.BeginAnimation(SolidColorBrush.ColorProperty, animation);
        }

        public unsafe void SetImage()
        {

            var bitmap = Util.ImageUtil.GetEXEIcon(Data.Path);
            if (bitmap == null)
            {
                //MessageBox.Show("无法获取图标（使用默认图标）", "错误");
                bitmap = DesktopUtility.Resources.Resource1.app_default_icon;
            }

            brush.Source = Util.ImageUtil.ToImageSource(bitmap);
        }

        private void back_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton.Equals(MouseButton.Left))
            {
                StartProcess();
            }
        }

        private void StartProcess()
        {
            if (Data.Path != String.Empty)
            {
                process = new Process();
                process.StartInfo.FileName = Data.Path;
                process.Start();
            }
        }


        private void StartItem_Click(object sender, RoutedEventArgs e)
        {
            StartProcess();
        }

        private void DeleteItem_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show($"删除 {Data} ?", "删除", MessageBoxButton.OKCancel);
            if (result == MessageBoxResult.OK)
            {
                DesktopUtility.Data.IconFactory.Remove(Data);
                if (App.Current.MainWindow != null)
                {
                    var window = (MainWindow)App.Current.MainWindow;
                    window.ReLayout();
                }
            }
        }

        private void RenameItem_Click(object sender, RoutedEventArgs e)
        {
            IconNameDialog dialog = new(Data.Name)
            {
                Title = "重命名"
            };
            dialog.ShowDialog();

            if (dialog.ok)
            {
                NameLabel.Text = dialog.IconName;
                Data.Name = dialog.IconName;
            }
        }
    }
}
