using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace DesktopUtility.Widget
{
    /// <summary>
    /// TaskDialog.xaml 的交互逻辑
    /// </summary>
    public partial class TaskDialog : Window
    {
        public Data.TaskData Data
        {
            get => new(contentInput.Text, false);
        }
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
            } else if (DesktopUtility.Data.TaskFactory.ExistByContent(contentInput.Text))
            {
                MessageBox.Show("任务已存在！", "错误");
            } else
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
