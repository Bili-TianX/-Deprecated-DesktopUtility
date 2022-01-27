using DesktopUtility.Util;
using DesktopUtility.Widget;
using System;
using System.Collections.Generic;
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

                var icon = new AppIcon(name, ofd.FileName);
                Data.IconFactory.icons.Add(icon);
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
}
