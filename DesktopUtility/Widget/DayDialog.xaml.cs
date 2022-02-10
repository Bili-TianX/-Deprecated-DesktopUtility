using System;
using System.Windows;

namespace DesktopUtility.Widget
{
    /// <summary>
    /// DayDialog.xaml 的交互逻辑
    /// </summary>
    public partial class DayDialog : Window
    {
        public bool ok = false;
        public Data.DayData Data => new(NameInput.Text, (DateTime)DateInput.SelectedDate);

        public DayDialog()
        {
            InitializeComponent();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(NameInput.Text))
            {
                MessageBox.Show("名称为空！", "错误");
            }
            else if (DesktopUtility.Data.DayFactory.ExistByName(NameInput.Text))
            {
                MessageBox.Show("重名！", "错误");
            }
            else if (DateInput.SelectedDate == null)
            {
                MessageBox.Show("未选择日期！", "错误");
            }
            else
            {
                ok = true;
                Close();
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
