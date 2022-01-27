using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.IO;
using System.Windows;

namespace DesktopUtility.Data
{
    public struct IconData
    {
        public string Name { get; set; }
        public string Path { get; set; }

        public IconData(string Name, string Path)
        {
            this.Name = Name;
            this.Path = Path;
        }
    }

    public static class IconFactory
    {
        public const string TargetFolder = "./data/icon/";
        public const string TargetFile = "icons.json";

        public static List<AppIcon> icons = new();

        public static bool ExistByName(string name)
        {
            bool result = false;
            foreach (var icon in icons)
            {
                if (icon.Data.Name == name)
                    result = true;
            }

            return result;
        }

        public static bool ExistByPath(string path)
        {
            bool result = false;
            icons.ForEach((icon) =>
            {
                if (icon.Data.Path == path)
                    result = true;
            });

            return result;
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
            var array = JArray.Parse(reader.ReadToEnd());
            foreach (var item in array)
            {
                var obj = JsonConvert.DeserializeObject(item.ToString(), typeof(IconData));
                if (obj != null)
                {
                    icons.Add(new AppIcon((IconData)obj));
                }
            }
        }

        public static void SaveToFile()
        {
            if (!Directory.Exists(TargetFolder))
            {
                Directory.CreateDirectory(TargetFolder);
            }
            StreamWriter writer = new(TargetFolder + TargetFile);
            JArray array = new();
            foreach(var icon in icons)
            {
                array.Add(JObject.FromObject(icon.Data));
            }
            writer.Write(array);
            writer.Close();
        }
    }
}
