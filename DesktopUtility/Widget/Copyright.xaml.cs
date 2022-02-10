using System.Diagnostics;
using System.Windows;

namespace DesktopUtility.Widget
{
    /// <summary>
    /// Copyright.xaml 的交互逻辑
    /// </summary>
    public partial class Copyright : Window
    {
        public Copyright()
        {
            InitializeComponent();
        }

        private void Hyperlink_Click(object sender, RoutedEventArgs e)
        {
            Dispatcher.BeginInvoke(() =>
            {
                Process p = new()
                {
                    StartInfo = new()
                    {
                        UseShellExecute = true,
                        FileName = "Explorer",
                        Arguments = "https://github.com/Bili-TianX/DesktopUtility"
                    }
                };
                p.Start();
            });
        }
    }
}
