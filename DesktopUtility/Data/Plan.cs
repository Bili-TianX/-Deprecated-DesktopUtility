using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;

namespace DesktopUtility.Data
{
    public class PlanData
    {
        public string title;
        public string content;
        public DateTime begin;
        public DateTime end;

        public bool contain(DateTime time)
        {
            return time.Day >= begin.Day && time.Day <= end.Day &&
                time.Month >= begin.Month && time.Month <= end.Month &&
                time.Year >= begin.Year && time.Year <= end.Year;
        }
    }

    public static class PlanFactory
    {
        public const string TargetFolder = "./data/plan/";
        public const string TargetFile = "plan.json";
        public static List<PlanData> plans = new();

        public static bool CheckExistByTitle(string title)
        {
            foreach (PlanData plan in plans)
            {
                if (plan.title == title)
                {
                    return true;
                }
            }

            return false;
        }

        public static void RemoveByTitle(string title)
        {
            foreach (PlanData plan in plans)
            {
                if (plan.title == title)
                {
                    plans.Remove(plan);
                    return;
                }
            }
        }

        public static PlanData? GetByTitle(string? title)
        {
            if (title == null)
            {
                return null;
            }

            foreach (PlanData plan in plans)
            {
                if (plan.title == title)
                {
                    return plan;
                }
            }

            return null;
        }

        public static void LoadFromFile()
        {
            if (!Directory.Exists(TargetFolder))
            {
                Directory.CreateDirectory(TargetFolder);
                return;
            }
            if (!File.Exists(TargetFolder + TargetFile))
            {
                return;
            }

            StreamReader reader = new(TargetFolder + TargetFile);
            JArray? array = JArray.Parse(reader.ReadToEnd());
            foreach (JToken? item in array)
            {
                object? obj = JsonConvert.DeserializeObject(item.ToString(), typeof(PlanData));
                if (obj != null)
                {
                    plans.Add((PlanData)obj);
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

            StreamWriter writer = new(TargetFolder + TargetFile);
            JArray array = new();
            foreach (PlanData plan in plans)
            {
                array.Add(JObject.FromObject(plan));
            }
            writer.Write(array);
            writer.Close();
        }
    }
}
