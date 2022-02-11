using Newtonsoft.Json.Linq;
using System.IO;

namespace DesktopUtility.Data
{
    internal class Setting
    {
        public static int Column_Count = 3;
        public static string Background_Image = "";
        public const string TargetFolder = "./data/";
        public const string TargetFile = "settings.json";

        public static void LoadSetting()
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

            using StreamReader reader = new(TargetFolder + TargetFile);
            JObject? o = JObject.Parse(reader.ReadToEnd());
            Column_Count = (int)(o["column_count"] ?? 3);
            Background_Image = (o["bg"] ?? "").ToString();
        }

        public static void SaveSetting()
        {
            JObject o = new();
            o["column_count"] = Column_Count;
            o["bg"] = Background_Image;

            using StreamWriter writer = new(TargetFolder + TargetFile);
            writer.Write(o.ToString());
        }
    }
}