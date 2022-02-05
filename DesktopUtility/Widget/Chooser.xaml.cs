using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace DesktopUtility.Widget
{
    /// <summary>
    /// Chooser.xaml 的交互逻辑
    /// </summary>
    public partial class Chooser : Window
    {
        public bool ok = false;
        private readonly List<(string name, string path)> ori;
        public List<(string name, string path)> result = new();

        public Chooser(List<(string name, string path)> list)
        {
            ori = list;
            InitializeComponent();

            foreach (var file in list)
            {
                ListBoxItem? item = new ListBoxItem();
                WrapPanel? panel = new WrapPanel();
                CheckBox? checkBox = new CheckBox()
                {
                    VerticalAlignment = VerticalAlignment.Center,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    LayoutTransform = new ScaleTransform()
                    {
                        ScaleX = 2,
                        ScaleY = 2,
                    }
                };
                Image? image = new Image()
                {
                    Source = Util.ImageUtil.ToImageSource(Util.ImageUtil.GetEXEIcon(file.path)),
                    VerticalAlignment = VerticalAlignment.Center,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    Width = 48,
                    Height = 48
                };
                TextBlock? textBlock = new TextBlock()
                {
                    Text = file.name,
                    VerticalAlignment = VerticalAlignment.Center,
                    HorizontalAlignment = HorizontalAlignment.Center
                };

                panel.Children.Add(checkBox);
                panel.Children.Add(image);
                panel.Children.Add(textBlock);

                item.Content = panel;
                listBox.Items.Add(item);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            foreach (ListBoxItem item in listBox.Items)
            {
                WrapPanel? panel = (WrapPanel)item.Content;
                CheckBox? box = (CheckBox)panel.Children[0];
                bool? state = box.IsChecked;
                if (state != null && (bool)state)
                {
                    string? name = ((TextBlock)panel.Children[2]).Text;
                    IEnumerable<(string name, string path)>? search = from i in ori
                                                    where i.name == name
                                                    select i;
                    if (search != null)
                    {
                        result.Add(search.ToList()[0]);
                    }
                }
            }
            ok = true;
            Close();
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            ok = false;
            Close();
        }
    }
}
