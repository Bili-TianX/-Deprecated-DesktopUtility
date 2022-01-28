using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.IO;

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

        public static bool operator ==(IconData a, IconData b)
        {
            return a.Name == b.Name && a.Path == b.Path;
        }

        public static bool operator !=(IconData a, IconData b)
        {
            return !(a == b);
        }

        public override string ToString()
        {
            return $"{Name} ({Path})";
        }

        public static bool CheckName(string NewName)
        {
            return NewName != string.Empty && !IconFactory.ExistByName(NewName);
        }
    }

    public static class IconFactory
    {
        public const string TargetFolder = "./data/icon/";
        public const string TargetFile = "icons.json";

        public static List<AppIcon> icons = new();

        public static void Remove(IconData data)
        {
            foreach (AppIcon? icon in icons)
            {
                if (icon.Data == data)
                {
                    icons.Remove(icon);
                    break;
                }
            }
        }

        public static bool ExistByName(string name)
        {
            bool result = false;
            foreach (AppIcon? icon in icons)
            {
                if (icon.Data.Name == name)
                {
                    result = true;
                }
            }

            return result;
        }

        public static bool ExistByPath(string path)
        {
            bool result = false;
            icons.ForEach((icon) =>
            {
                if (icon.Data.Path == path)
                {
                    result = true;
                }
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
            JArray? array = JArray.Parse(reader.ReadToEnd());
            foreach (JToken? item in array)
            {
                object? obj = JsonConvert.DeserializeObject(item.ToString(), typeof(IconData));
                if (obj != null)
                {
                    icons.Add(new AppIcon((IconData)obj));
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
            foreach (AppIcon? icon in icons)
            {
                array.Add(JObject.FromObject(icon.Data));
            }
            writer.Write(array);
            writer.Close();
        }
    }
}
