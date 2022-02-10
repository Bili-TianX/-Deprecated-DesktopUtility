using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DesktopUtility.Data
{
    public class DayData
    {
        public DateTime time;
        public string name;

        public DayData(string name, DateTime time)
        {
            this.name = name;
            this.time = time;
        }

        public object DayFactory { get; internal set; }
    }

    public class DayFactory
    {
        public const string TargetFolder = "./data/day/";
        public const string TargetFile = "days.json";
        public static List<DayData> list = new();

        public static bool ExistByName(string name)
        {
            return (from i in list
                    where i.name == name
                    select i).Any();
        }

        public static void LoadFromFile()
        {
            if (!Directory.Exists(TargetFolder))
            {
                Directory.CreateDirectory(TargetFolder);
                return;
            }

            if (!System.IO.File.Exists(TargetFolder + TargetFile))
            {
                return;
            }

            StreamReader reader = new(TargetFolder + TargetFile);
            JArray? array = JArray.Parse(reader.ReadToEnd());
            foreach (JToken? item in array)
            {
                object? obj = JsonConvert.DeserializeObject(item.ToString(), typeof(DayData));
                if (obj != null)
                {
                    list.Add((DayData)obj);
                }
            }

            reader.Close();
        }

        public static void SaveToFile()
        {
            if (!Directory.Exists(TargetFolder))
            {
                Directory.CreateDirectory(TargetFolder);
            }
            using (StreamWriter writer = new(TargetFolder + TargetFile))
            {
                JArray array = new();
                foreach (DayData? item in list)
                {
                    array.Add(JObject.FromObject(item));
                }
                writer.Write(array);
            }
        }

    }
}
