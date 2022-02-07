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
    /// DayDialog.xaml 的交互逻辑
    /// </summary>
    public partial class DayDialog : Window
    {
        public bool ok = false;
        public Data.DayData Data
        {
#pragma warning disable CS8629 // 可为 null 的值类型可为 null。
            get => new(NameInput.Text, (DateTime)DateInput.SelectedDate);
#pragma warning restore CS8629 // 可为 null 的值类型可为 null。
        }

        public DayDialog()
        {
            InitializeComponent();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(NameInput.Text))
            {
                MessageBox.Show("名称为空！", "错误");
            } else if (DesktopUtility.Data.DayFactory.ExistByName(NameInput.Text))
            {
                MessageBox.Show("重名！", "错误");
            } else if (DateInput.SelectedDate == null) {
                MessageBox.Show("未选择日期！", "错误");
            } else
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
