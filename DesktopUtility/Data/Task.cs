using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DesktopUtility.Data
{
    public class TaskData
    {
        public string content;
        
        public TaskData(string content)
        {
            this.content = content;
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
            foreach (JToken? item in array)
            {
                object? obj = JsonConvert.DeserializeObject(item.ToString(), typeof(TaskData));
                if (obj != null)
                {
                    list.Add((TaskData)obj);
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
                foreach (var item in list)
                {
                    array.Add(JObject.FromObject(item));
                }
                writer.Write(array);
            }
        }
    }
}
