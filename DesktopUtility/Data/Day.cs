using System;
using System.Collections.Generic;

namespace DesktopUtility.Data
{
    internal class DayData
    {
        public DateTime time;
        public String name;

        public DayData(String name, DateTime time)
        {
            this.name = name;
            this.time = time;
        }
    }

    internal class DayFactory
    {
        public const string TargetFolder = "./data/day/";
        public const string TargetFile = "days.json";
        public static List<DayData> list = new();

        public static void loadFromFile()
        {

        }

        public static void saveToFile()
        {

        }
    }
}
