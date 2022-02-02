using System;
using System.Windows;
using System.Windows.Controls;

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
            list.Items.Add(new ListBoxItem()
            {
                Style = itemStyle,
                Content = data.title
            });
        }

        private void list_Selected(object sender, RoutedEventArgs e)
        {
            ListBoxItem? item = (ListBoxItem?)list.SelectedItem;
            if (item == null)
            {
                return;
            }

            Data.PlanData? plan = Data.PlanFactory.GetByTitle(item.Content.ToString());
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
                dialog.titleBox.Text = ((ListBoxItem)item).Content.ToString();
                dialog.title.Text = "修改计划";
                dialog.ShowDialog();
                Data.PlanData? Item = Data.PlanFactory.GetByTitle(((ListBoxItem)item).Content.ToString());
                if (dialog.ok)
                {
                    Data.PlanData? data = dialog.Data;

#pragma warning disable CS8602 // 解引用可能出现空引用。
                    Item.title = data.title;
#pragma warning restore CS8602 // 解引用可能出现空引用。
                    Item.begin = data.begin;
                    Item.end = data.end;
                    Item.content = data.content;

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
                if (MessageBox.Show($"删除计划 \"{Item.Content}\" ?", "删除计划", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    Data.PlanFactory.RemoveByTitle(Item.Content.ToString());
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

        private void addItem_Click(object sender, RoutedEventArgs e)
        {
            PlanDialog? dialog = new PlanDialog();
            dialog.startTimeBox.Text = time.ToString();
            dialog.ShowDialog();
            if (dialog.ok)
            {
                Data.PlanFactory.plans.Add(dialog.Data);
                UpdatePlans();
                ((MainWindow)App.Current.MainWindow).AttachPlan();
                startBlock.Text = endBlock.Text = contentBlock.Text = "<空>";
            }
        }
    }
#pragma warning restore CS8604 // 引用类型参数可能为 null。
}
