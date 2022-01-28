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
        public const int COLUMN_COUNT = 1;
        public static Style? boxItemStyle;

        public MainWindow()
        {
            InitializeComponent();

            var values = App.Current.MainWindow.Resources.Values;
            foreach (var value in values)
            {
                if (value.GetType() == typeof(Style) && ((Style)value).TargetType.Name == nameof(ListBoxItem))
                {
                    boxItemStyle = (Style)value;
                    break;
                }
            }

            for (int i = 0;  i < COLUMN_COUNT; i++)
            {
                LaunchPad.ColumnDefinitions.Add(new ColumnDefinition()
                {
                    Width = new GridLength(1, GridUnitType.Star)
                });
            }
            Icon = Util.ImageUtil.ToImageSource(DesktopUtility.Resources.Resource1.icon);
            addAppItem.Icon = Util.ImageUtil.ToImage(DesktopUtility.Resources.Resource1.addIcon);

            ReLayout();
        }

        public void ReLayout()
        {
            LaunchPad.Children.Clear();
            LaunchPad.RowDefinitions.Clear();


            for (int i = 0; i < Data.IconFactory.icons.Count; ++i)
            {
                AddIcon(Data.IconFactory.icons[i], i);
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
            if (Data.IconFactory.ExistByPath(ofd.FileName))
            {
                MessageBox.Show("应用已存在: " + ofd.FileName, "错误");
                return;
            }
            if (r != null && (bool)r)
            {
                var dialog = new IconNameDialog(ofd.SafeFileName[..^4].Capitalize());
                dialog.ShowDialog();
                if (!dialog.ok) return;
                var name = dialog.IconName;

                AddIcon(new Data.IconData(name, ofd.FileName));
            }
        }

        private void AddIcon(Data.IconData data)
        {
            var icon = new AppIcon(data);
            Data.IconFactory.icons.Add(icon);
            AddIcon(icon);
        }

        private void AddIcon(AppIcon icon, int index = -1)
        {
            if (index == -1)
            {
                index = Data.IconFactory.icons.Count - 1;
            }

            LaunchPad.Children.Add(icon);

            if (LaunchPad.RowDefinitions.Count <= index / COLUMN_COUNT)
            {
                LaunchPad.RowDefinitions.Add(new RowDefinition());
            }

            Grid.SetColumn(icon, index % COLUMN_COUNT);
            Grid.SetRow(icon, index / COLUMN_COUNT);

            icon.SetImage();
        }
    }
}
