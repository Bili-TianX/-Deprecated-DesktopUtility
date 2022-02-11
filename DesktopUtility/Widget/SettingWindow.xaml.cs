using System;
using System.IO;
using System.Windows;

namespace DesktopUtility.Widget
{
    /// <summary>
    /// SettingWindow.xaml 的交互逻辑
    /// </summary>
    public partial class SettingWindow : Window
    {
        public SettingWindow()
        {
            InitializeComponent();
            ColumnBox.Text = Data.Setting.Column_Count.ToString();
            BGBox.Text = Data.Setting.Background_Image;
        }

        private void NoButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void YesButton_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(ColumnBox.Text, out int res))
            {
                Data.Setting.Column_Count = res;
                Data.Setting.Background_Image = BGBox.Text;

                if (File.Exists(Data.Setting.Background_Image))
                {
                    File.Copy(Data.Setting.Background_Image, "./data/bg.png", true);
                    Data.Setting.Background_Image = "./data/bg.png";
                }

                MainWindow? w = App.Current.MainWindow as MainWindow;
                w?.ReLayout();
                w?.updateBackground();

                Close();
            }
            else
            {
                MessageBox.Show("列数有误");
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog ofd = new()
            {
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.CommonPictures),
                Filter = "(*.jpg;*.png)|*.jpg;*.png",
                Multiselect = false
            };
            bool? r = ofd.ShowDialog(this);

            if (r != null && (bool)r)
            {
                BGBox.Text = ofd.FileName;
            }
        }
    }
}
