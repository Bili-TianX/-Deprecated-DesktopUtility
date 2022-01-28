using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DesktopUtility.Widget
{
    /// <summary>
    /// MyCalendar.xaml 的交互逻辑
    /// </summary>
    public partial class MyCalendar : UserControl
    {
        public int year;
        public int month;

        public MyCalendar()
        {
            InitializeComponent();

            var now = DateTime.Now;
            year = now.Year;
            month = now.Month;
            var week = ((int)new DateTime(year, month, 1).DayOfWeek);
        }
    }
}
