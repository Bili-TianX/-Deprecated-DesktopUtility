using System;
using System.Windows;
using System.Windows.Media;

namespace DesktopUtility.Widget
{
    /// <summary>
    /// IconNameDialog.xaml 的交互逻辑
    /// </summary>
    public partial class IconNameDialog : Window
    {
        public bool ok = false;

        public IconNameDialog()
        {
            InitializeComponent();
            Height = 144;
            Width = 324;
            Icon = Util.ImageUtil.ToImageSource(DesktopUtility.Resources.Resource1.icon);
            icon.Source = Util.ImageUtil.ToImageSource(DesktopUtility.Resources.Resource1.question);
        }

        public IconNameDialog(string defaultName) : this()
        {
            IconName = defaultName;
        }

        public string IconName
        {
            get => box.Text;
            set => box.Text = value;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (!Data.IconData.CheckName(IconName))
            {
                label.Text = "非法名称：请重新输入";
                label.Foreground = new SolidColorBrush(Util.ColorUtil.Red);
            }
            else
            {
                ok = true;
                Close();
            }
        }

        private void CancelButtonClicked(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
