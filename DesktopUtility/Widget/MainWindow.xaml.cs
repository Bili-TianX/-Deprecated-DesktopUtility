using DesktopUtility.Util;
using DesktopUtility.Widget;
using System;
using System.Windows;
using System.Windows.Controls;

namespace DesktopUtility
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Icon = Util.ImageUtil.ToImageSource(DesktopUtility.Resources.Resource1.icon);
            addAppItem.Icon = Util.ImageUtil.ToImage(DesktopUtility.Resources.Resource1.addIcon);
            foreach (var icon in Data.IconFactory.icons)
            {
                addIcon(icon);
            }
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog ofd = new()
            {
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.CommonPrograms),
                Filter = "(*.exe)|*.exe",
                Multiselect = false
            };
            var r = ofd.ShowDialog(this);
            if (r != null && (bool)r)
            {
                var dialog = new IconNameDialog(ofd.SafeFileName[..^4].Capitalize());
                dialog.ShowDialog();
                if (!dialog.ok) return;
                var name = dialog.IconName;

                addIcon(new Data.IconData(name, ofd.FileName));
            }
        }

        private void addIcon(Data.IconData data)
        {
            var icon = new AppIcon(data);
            Data.IconFactory.icons.Add(icon);
            addIcon(icon);
        }

        private void addIcon(AppIcon icon)
        {
            LaunchPad.Children.Add(icon);

            int len = Data.IconFactory.icons.Count;
            if (LaunchPad.RowDefinitions.Count <= (len - 1) / 3)
            {
                LaunchPad.RowDefinitions.Add(new RowDefinition());
            }

            Grid.SetColumn(icon, (len - 1) % 3);
            Grid.SetRow(icon, (len - 1) / 3);

            icon.SetImage();
        }
    }
}
