using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace DesktopUtility.Widget
{
    /// <summary>
    /// DateLabel.xaml 的交互逻辑
    /// </summary>
    public partial class DateLabel : UserControl
    {
        public DateLabel()
        {
            InitializeComponent();
        }

        private void Label_MouseEnter(object sender, MouseEventArgs e)
        {
            border.BorderBrush = new SolidColorBrush(Util.ColorUtil.Black);
            border.Background = new SolidColorBrush(Util.ColorUtil.LightGray);
        }

        private void Label_MouseLeave(object sender, MouseEventArgs e)
        {
            border.BorderBrush = new SolidColorBrush(Util.ColorUtil.Transparent);
            border.Background = new SolidColorBrush(Util.ColorUtil.Transparent);
        }
    }


}
