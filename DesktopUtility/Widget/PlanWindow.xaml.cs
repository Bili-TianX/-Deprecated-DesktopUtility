using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace DesktopUtility.Widget
{
    /// <summary>
    /// PlanWindow.xaml 的交互逻辑
    /// </summary>
    public partial class PlanWindow : Window
    {
        private readonly Style itemStyle;
        private readonly DateTime? time = null;

        public PlanWindow()
        {
            InitializeComponent();
            list.SelectionChanged += list_Selected;
            foreach (object? item in Resources.Values)
            {
                if (item is Style)
                {
                    itemStyle = (Style)item;
                    break;
                }
            }
            UpdatePlans();

            addItem.Icon = Util.ImageUtil.ToImage(DesktopUtility.Resources.Resource1.addIcon);
            modifyItem.Icon = Util.ImageUtil.ToImage(DesktopUtility.Resources.Resource1.renameIcon);
            deleteItem.Icon = Util.ImageUtil.ToImage(DesktopUtility.Resources.Resource1.deleteIcon);
        }

        public PlanWindow(DateTime time)
        {
            this.time = new DateTime(time.Year, time.Month, time.Day);
            InitializeComponent();
            list.SelectionChanged += list_Selected;
            foreach (object? item in Resources.Values)
            {
                if (item is Style)
                {
                    itemStyle = (Style)item;
                    break;
                }
            }
            UpdatePlans();

            addItem.Icon = Util.ImageUtil.ToImage(DesktopUtility.Resources.Resource1.addIcon);
            modifyItem.Icon = Util.ImageUtil.ToImage(DesktopUtility.Resources.Resource1.renameIcon);
            deleteItem.Icon = Util.ImageUtil.ToImage(DesktopUtility.Resources.Resource1.deleteIcon);
        }

        public void UpdatePlans()
        {
            list.Items.Clear();
            foreach (Data.PlanData plan in Data.PlanFactory.plans)
            {
                if (time != null)
                {
                    if (plan.contain((DateTime)time))
                    {
                        ListAddPlan(plan);
                    }
                }
                else
                {
                    ListAddPlan(plan);
                }

            }
        }

        public void ListAddPlan(Data.PlanData data)
        {
            WrapPanel panel = new();
            CheckBox box = new()
            {
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center,
                LayoutTransform = new ScaleTransform()
                {
                    ScaleX = 2,
                    ScaleY = 2,
                },
                IsChecked = data.check
            };
            box.Click += (o, e) =>
            {
                data.check = (bool)box.IsChecked;
            };
            TextBlock block = new() { Text = data.title };
            panel.Children.Add(box);
            panel.Children.Add(block);

            list.Items.Add(new ListBoxItem()
            {
                Style = itemStyle,
                Content = panel
            });
        }

        private string GetTitle(ListBoxItem item)
        {
            return ((TextBlock)((WrapPanel)item.Content).Children[1]).Text;
        }

        private void list_Selected(object sender, RoutedEventArgs e)
        {
            ListBoxItem? item = (ListBoxItem?)list.SelectedItem;
            if (item == null)
                return;

            Data.PlanData? plan = Data.PlanFactory.GetByTitle(GetTitle(item));
            if (plan == null)
            {
                return;
            }

            contentBlock.Text = plan?.content;
            startBlock.Text = plan?.begin.ToString();
            endBlock.Text = plan?.end.ToString();
        }

#pragma warning disable CS8604 // 引用类型参数可能为 null。
        private void modifyItem_Click(object sender, RoutedEventArgs e)
        {
            object? item = list.SelectedItem;
            if (item != null)
            {
                PlanDialog? dialog = new PlanDialog(true);
                dialog.titleBox.Text = GetTitle((ListBoxItem)item);
                dialog.title.Text = "修改计划";
                dialog.ShowDialog();
                Data.PlanData? Item = Data.PlanFactory.GetByTitle(GetTitle((ListBoxItem)item));
                if (dialog.ok)
                {
                    Data.PlanData? data = dialog.Data;

#pragma warning disable CS8602 // 解引用可能出现空引用。
                    Item.title = data.title;
#pragma warning restore CS8602 // 解引用可能出现空引用。
                    Item.begin = data.begin;
                    Item.end = data.end;
                    Item.content = data.content;
                    Item.check = false;

                    UpdatePlans();
                    ((MainWindow)App.Current.MainWindow).AttachPlan();
                    startBlock.Text = endBlock.Text = contentBlock.Text = "<空>";
                }
            }
            else
            {
                MessageBox.Show("未选择计划", "错误");
            }
        }

        private void deleteItem_Click(object sender, RoutedEventArgs e)
        {
            object? item = list.SelectedItem;
            if (item != null)
            {
                ListBoxItem? Item = (ListBoxItem)item;
                if (MessageBox.Show($"删除计划 \"{GetTitle(Item)}\" ?", "删除计划", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    Data.PlanFactory.RemoveByTitle(GetTitle(Item));
                    UpdatePlans();
                    ((MainWindow)App.Current.MainWindow).calendar.AttachPlan();
                    startBlock.Text = endBlock.Text = contentBlock.Text = "<空>";
                }
            }
            else
            {
                MessageBox.Show("未选择计划", "错误");
            }
        }

        private void addItem_Click(object sender, RoutedEventArgs e)
        {
            PlanDialog? dialog = new PlanDialog();
            dialog.startTimeBox.Text = DateTime.Now.ToString();
            dialog.ShowDialog();
            if (dialog.ok)
            {
                Data.PlanFactory.plans.Add(dialog.Data);
                UpdatePlans();
                ((MainWindow)App.Current.MainWindow).calendar.AttachPlan();
                startBlock.Text = endBlock.Text = contentBlock.Text = "<空>";
            }
        }
    }
#pragma warning restore CS8604 // 引用类型参数可能为 null。
}
