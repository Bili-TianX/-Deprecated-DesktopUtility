using System.Windows;

namespace DesktopUtility.Widget
{
    /// <summary>
    /// TaskDialog.xaml 的交互逻辑
    /// </summary>
    public partial class TaskDialog : Window
    {
        public Data.TaskData Data => new(contentInput.Text, false);
        public bool ok = false;
        public TaskDialog()
        {
            InitializeComponent();
        }

        private void yesButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(contentInput.Text))
            {
                MessageBox.Show("缺少内容！", "错误");
            }
            else if (DesktopUtility.Data.TaskFactory.ExistByContent(contentInput.Text))
            {
                MessageBox.Show("任务已存在！", "错误");
            }
            else
            {
                ok = true;
                Close();
            }
        }

        private void noButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
