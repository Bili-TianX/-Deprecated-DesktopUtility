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
        }

        public void UpdatePlans()
        {
            foreach (Data.PlanData plan in Data.PlanFactory.plans)
            {
                ListAddPlan(plan);
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

        private void modifyItem_Click(object sender, RoutedEventArgs e)
        {

        }

        private void deleteItem_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
