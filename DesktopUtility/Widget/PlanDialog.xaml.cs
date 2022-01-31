using System;
using System.Windows;

namespace DesktopUtility.Widget
{
    /// <summary>
    /// PlanDialog.xaml 的交互逻辑
    /// </summary>
    public partial class PlanDialog : Window
    {
        public bool ok = false;
        public Data.PlanData Data => new Data.PlanData()
        {
            title = titleBox.Text,
            content = contentBox.Text,
            begin = DateTime.Parse(startTimeBox.Text),
            end = DateTime.Parse(endTimeBox.Text)
        };

        public PlanDialog()
        {
            InitializeComponent();
            DateTime time = System.DateTime.Now;
            startTimeBox.Text = time.ToString();
            endTimeBox.Text = time.AddDays(1).ToString();
            startBlock.ToolTip = "格式: 年/月/日 时:分:秒";
            endBlock.ToolTip = "格式: 年/月/日 时:分:秒";
        }

        private void okButton_Click(object sender, RoutedEventArgs e)
        {
            if (titleBox.Text.Length == 0)
            {
                MessageBox.Show("标题为空！", "错误");
                return;
            }
            else
            {
                ok = true;
                Close();
            }
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void endTimeBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            bool result = System.DateTime.TryParse(endTimeBox.Text, out _);
            if (!result)
            {
                endBlock.Foreground = new System.Windows.Media.SolidColorBrush(Util.ColorUtil.Red);
                endBlock.Text = "结束时间*";
                okButton.IsEnabled = false;
            }
            else
            {
                endBlock.Foreground = new System.Windows.Media.SolidColorBrush(Util.ColorUtil.Black);
                endBlock.Text = "结束时间";
                okButton.IsEnabled = true;
            }
        }

        private void startTimeBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            bool result = System.DateTime.TryParse(startTimeBox.Text, out DateTime date);
            if (!result)
            {
                startBlock.Foreground = new System.Windows.Media.SolidColorBrush(Util.ColorUtil.Red);
                startBlock.Text = "开始时间*";
                okButton.IsEnabled = false;
            }
            else
            {
                startBlock.Foreground = new System.Windows.Media.SolidColorBrush(Util.ColorUtil.Black);
                startBlock.Text = "开始时间";
                endTimeBox.Text = date.AddDays(1).ToString();
                okButton.IsEnabled = true;
            }
        }
    }
}
