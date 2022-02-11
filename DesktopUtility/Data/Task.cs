using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DesktopUtility.Data
{
    public class TaskData
    {
        public string content;
        public bool check;

        public TaskData(string content, bool check)
        {
            this.content = content;
            this.check = check;
        }
    }

    public class TaskFactory
    {
        public const string TargetFolder = "./data/task/";
        public const string TargetFile = "tasks.json";
        public static List<TaskData> list = new();

        public static bool ExistByContent(string content)
        {
            return (from i in list
                    where i.content == content
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
            bool first = true;
            bool flag = false;
            foreach (JToken? item in array)
            {
                if (first)
                {
                    first = false;
                    DateTime last = DateTime.Parse((string?)item);
                    DateTime now = DateTime.Now;
                    if (new DateTime(now.Year, now.Month, now.Day) > new DateTime(last.Year, last.Month, last.Day))
                    {
                        flag = true;
                    }
                }
                else
                {
                    object? obj = JsonConvert.DeserializeObject(item.ToString(), typeof(TaskData));
                    TaskData? tmp = (TaskData)obj;
                    if (flag)
                    {
                        tmp.check = false;
                    }

                    list.Add(tmp);
                }
            }

            reader.Close();
        }

        public static List<TaskData> Unfinished()
        {
            return (from item in list
                    where item.check == false
                    select item).ToList();
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
                array.Add(DateTime.Now.ToString());
                foreach (TaskData? item in list)
                {
                    array.Add(JObject.FromObject(item));
                }

                writer.Write(array);
            }
        }
    }
}
