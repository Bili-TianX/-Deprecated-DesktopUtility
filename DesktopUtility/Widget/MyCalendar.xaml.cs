﻿using System;
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
        public int max_day;
        public int row_count;
        public int week;

        public MyCalendar()
        {
            InitializeComponent();

            var now = DateTime.Now;
            year = now.Year;
            month = now.Month;
            week = ((int)new DateTime(year, month, 1).DayOfWeek);
            max_day = Util.DateUtil.GetMaxDay(year, month);
            row_count = (int) Math.Ceiling((max_day + week) / 7.0);

            ReLayout();
        }

        public void ReLayout()
        {
            MainGrid.RowDefinitions.Clear();
            MainGrid.RowDefinitions.Add(new RowDefinition()
            {
                Height = new GridLength(48, GridUnitType.Pixel)
            });
            MainGrid.RowDefinitions.Add(new RowDefinition()
            {
                Height = new GridLength(0.75, GridUnitType.Star)
            });

            for (int i = 0; i < row_count; ++i)
            {
                MainGrid.RowDefinitions.Add(new RowDefinition()
                {
                    Height = new GridLength(1, GridUnitType.Star)
                });
            }
            Grid.SetRowSpan(border, MainGrid.RowDefinitions.Count);

            for (int i = 1; i <= max_day; ++i)
            {
                var label = new Label()
                {
                    Content = i,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center
                };
                MainGrid.Children.Add(label);
                Grid.SetRow(label, 2 + (i + week - 2) / 7);
                Grid.SetColumn(label, (i + week - 2) % 7);
            }
        }
    }
}