using DesktopUtility.Widget;
using IWshRuntimeLibrary;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
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

        public override bool Equals(object? obj)
        {
            if (obj == null)
            {
                return false;
            }
            else if (obj.GetType() != typeof(IconData))
            {
                return false;
            }
            else
            {
                return this == (IconData)obj;
            }
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
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

        private static List<(string name, string path)> getAll()
        {
            string? path = Environment.GetFolderPath(Environment.SpecialFolder.CommonPrograms);
            Stack<DirectoryInfo> dirs = new();
            List<FileInfo> files = new();
            List<(string name, string path)> result = new();
            WshShell shell = new WshShell();

            dirs.Push(new DirectoryInfo(path));
            while (dirs.Count > 0)
            {
                DirectoryInfo? dir = dirs.Pop();
                foreach (DirectoryInfo? dirInfo in dir.GetDirectories())
                {
                    dirs.Push(dirInfo);
                }

                foreach (FileInfo? file in dir.GetFiles())
                {
                    files.Add(file);
                }
            }

            foreach (FileInfo? file in files)
            {
                if (file.Extension == ".lnk")
                {
                    try
                    {
                        FileInfo? target = new FileInfo(((IWshShortcut)shell.CreateShortcut(file.FullName)).TargetPath);
                        if (target.Extension == ".exe")
                        {
                            result.Add((file.Name[..^4], target.FullName));
                        }
                    }
                    catch (Exception)
                    {
                        continue;
                    }
                }
            }

            return result;
        }

        public static void LoadFromFile()
        {
            if (!Directory.Exists(TargetFolder))
            {
                Directory.CreateDirectory(TargetFolder);
            }

            if (!System.IO.File.Exists(TargetFolder + TargetFile))
            {
                List<(string name, string path)>? list = getAll();
                Chooser? dialog = new Chooser(list);
                dialog.ShowDialog();
                if (dialog.ok)
                {
                    foreach (var item in dialog.result)
                    {
                        icons.Add(new AppIcon(new IconData(item.name, item.path)));
                    }
                }
            }
            else
            {
                StreamReader reader = new(TargetFolder + TargetFile);
                JArray? array = JArray.Parse(reader.ReadToEnd());
                foreach (JToken? item in array)
                { 
                    IconData? obj = (IconData?)JsonConvert.DeserializeObject(item.ToString(), typeof(IconData));
                    if (obj != null)
                    {
                        if (!System.IO.File.Exists(obj?.Path))
                        {
                            MessageBox.Show($"无法找到{obj?.Name}({obj?.Path}), 请查看软件是否已卸载", "警告");
                            continue;
                        }
                        icons.Add(new AppIcon((IconData)obj));
                    }
                }

                reader.Close();
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
            foreach (AppIcon? icon in icons)
            {
                array.Add(JObject.FromObject(icon.Data));
            }
            writer.Write(array);
            writer.Close();
        }
    }
}
