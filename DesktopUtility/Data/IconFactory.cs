using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesktopUtility.Data
{
    public struct IconData
    {
        public string Name { get; set; }
        public string Path { get; set; }

        IconData(string Name, string Path)
        {
            this.Name = Name;
            this.Path = Path;
        }
    }

    public static class IconFactory
    {
        public static List<AppIcon> icons = new();

        public static bool ExistByName(string name)
        {
            bool result = false;
            foreach (var icon in icons)
            {
                if (icon.Name == name)
                    result = true;
            }

            return result;
        }

        public static bool ExistByPath(string path)
        {
            bool result = false;
            icons.ForEach((icon) =>
            {
                if (icon.data.Path == path)
                    result = true;
            });

            return result;
        }
    }
}
