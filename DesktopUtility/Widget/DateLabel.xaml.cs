using System;
using System.Windows;
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
        public bool attached = false;
        public DateTime Time;

        public DateLabel(int year, int month, int day)
        {
            InitializeComponent();
            Time = new(year, month, day);
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

        public void AttachPlan()
        {
            attached = true;
            block.Foreground = new SolidColorBrush(Color.FromRgb(123, 131, 167));
            block.FontWeight = FontWeights.ExtraBold;
        }

        public void RemovePlan()
        {
            attached = false;
            block.Foreground = new SolidColorBrush(Color.FromRgb(0x80, 0x00, 0x80));
            block.FontWeight = FontWeights.Normal;
        }

        private void Label_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ((MainWindow)Application.Current.MainWindow).ShowPlan(Time);
        }
    }
}
