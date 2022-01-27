using System;
using System.Diagnostics;
using System.Drawing;
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
        public readonly Data.IconData data;
        public const int duration = 200;

        public AppIcon()
        {
            InitializeComponent();
            data = new()
            {
                Path = "",
                Name = ""
            };
            border.Background = new SolidColorBrush(System.Windows.Media.Color.FromArgb(0, 0, 0, 0));

            StartItem.Icon = Util.ImageUtil.ToImage(DesktopUtility.Resources.Resource1.startIcon);
            DeleteItem.Icon = Util.ImageUtil.ToImage(DesktopUtility.Resources.Resource1.deleteIcon);
            RenameItem.Icon = Util.ImageUtil.ToImage(DesktopUtility.Resources.Resource1.renameIcon);
        }

        public AppIcon(String name, String path) : this()
        {
            data = new()
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
                To = System.Windows.Media.Color.FromArgb(200, 211, 211, 211),
                Duration = TimeSpan.FromMilliseconds(duration),
                AccelerationRatio = 1
            };


            border.Background.BeginAnimation(SolidColorBrush.ColorProperty, animation);
        }


        private void back_MouseLeave(object sender, MouseEventArgs e)
        {
            ColorAnimation animation = new()
            {
                To = Util.ColorUtil.Transparent,
                From = System.Windows.Media.Color.FromArgb(200, 211, 211, 211),
                Duration = TimeSpan.FromMilliseconds(duration),
                AccelerationRatio = 1
            };


            border.Background.BeginAnimation(SolidColorBrush.ColorProperty, animation);
        }

        public unsafe void SetImage()
        {

            var bitmap = Util.ImageUtil.GetEXEIcon(data.Path);
            if (bitmap == null)
            {
                MessageBox.Show("无法获取图标（使用默认图标）", "错误");
                bitmap = DesktopUtility.Resources.Resource1.app_default_icon;
            }

            brush.Source = Util.ImageUtil.ToImageSource(bitmap);
        }

        private void back_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton.Equals(MouseButton.Left))
            {
                if (data.Path != "" && data.Path != null)
                {
                    var p = new Process();
                    p.StartInfo.FileName = data.Path;
                    p.Start();
                }
            }

        }

        private void StartItem_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(brush.GetType().ToString());
        }

        private void DeleteItem_Click(object sender, RoutedEventArgs e)
        {

        }

        private void RenameItem_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
