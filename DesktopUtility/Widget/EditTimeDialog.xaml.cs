using System.Windows;
using System.Windows.Controls;

namespace DesktopUtility.Widget
{
    /// <summary>
    /// EditTimeDialog.xaml 的交互逻辑
    /// </summary>
    public partial class EditTimeDialog : Window
    {
        public bool ok = false;
        public int Year
        {
            get
            {
                ComboBoxItem? item = (ComboBoxItem)yearBox.SelectedItem;
                if (item != null)
                {
                    return int.Parse(item.Content.ToString());
                }
                else
                {
                    return -1;
                }
            }
        }
        public int Month
        {
            get
            {
                ComboBoxItem? item = (ComboBoxItem)monthBox.SelectedItem;
                if (item != null)
                {
                    return int.Parse(item.Content.ToString());
                }
                else
                {
                    return -1;
                }
            }
        }

        public EditTimeDialog(int y, int m)
        {
            InitializeComponent();

            for (int i = 1900; i <= 2100; ++i)
            {
                yearBox.Items.Add(new ComboBoxItem()
                {
                    Content = i.ToString(),
                    VerticalAlignment = VerticalAlignment.Center,
                    HorizontalAlignment = HorizontalAlignment.Center
                });
            }

            for (int i = 1; i <= 12; ++i)
            {
                monthBox.Items.Add(new ComboBoxItem()
                {
                    Content = i.ToString(),
                    VerticalAlignment = VerticalAlignment.Center,
                    HorizontalAlignment = HorizontalAlignment.Center
                });
            }

            yearBox.SelectedIndex = y - 1900;
            monthBox.SelectedIndex = m - 1;
        }

        private void yesButton_Click(object sender, RoutedEventArgs e)
        {
            int year = Year;
            int month = Month;
            if (year == -1 || month == -1)
            {
                MessageBox.Show("未选择月份/年份", "错误");
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
