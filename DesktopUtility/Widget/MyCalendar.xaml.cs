﻿using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

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
        public List<DateLabel> list = new();

        public MyCalendar()
        {
            InitializeComponent();

            DateTime now = DateTime.Now;
            year = now.Year;
            month = now.Month;
            SetTime(now.Year, now.Month);
            title.Text = $"{year}年{month}月";
        }

        public void SetTime(int year, int month)
        {
            week = ((int)new DateTime(year, month, 1).DayOfWeek);
            max_day = Util.DateUtil.GetMaxDay(year, month);
            row_count = (int)Math.Ceiling((max_day + week) / 7.0);

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
            MainGrid.RowDefinitions.Add(new RowDefinition()
            {
                Height = new GridLength(10, GridUnitType.Pixel)
            });
            Grid.SetRowSpan(border, MainGrid.RowDefinitions.Count);

            list.Clear();
            for (int i = 1; i <= max_day; ++i)
            {
                Widget.DateLabel? label = new(year, month, i);
                label.block.Text = i.ToString();
                MainGrid.Children.Add(label);
                Grid.SetRow(label, 2 + (i + week - 2) / 7);
                Grid.SetColumn(label, (i + week - 2) % 7);
                list.Add(label);
            }

        }
    }
}
