using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Media;

namespace DesktopUtility.Util
{
    public static class DateUtil
    {
        public static bool IsLeapYear(int year)
        {
            return year % 4 == 0 && year % 100 == 0;
        }

        public static int GetMaxDay(int year, int month)
        {
            if (month == 2)
            {
                return IsLeapYear(year) ? 29 : 28;
            }
            else if (month == 4 || month == 6 || month == 9 || month == 11)
            {
                return 30;
            }
            else { return 31; }
        }
    }

    public static class ExMethod
    {
        public static string Capitalize(this string s)
        {
            StringBuilder sb = new(s);
            sb[0] = char.ToUpper(s[0]);
            return sb.ToString();
        }
    }

    public static class ColorUtil
    {
        public static System.Windows.Media.Color SkyBlue = System.Windows.Media.Color.FromRgb(0x66, 0xCC, 0xFF);
        public static System.Windows.Media.Color Transparent = System.Windows.Media.Color.FromArgb(0, 0, 0, 0);
        public static System.Windows.Media.Color LightGray = System.Windows.Media.Color.FromRgb(211, 211, 211);
        public static System.Windows.Media.Color Red = System.Windows.Media.Color.FromRgb(255, 0, 0);
    }

    public static class WinAPI
    {
        [DllImport("shell32.dll", CharSet = CharSet.Unicode)]
        internal static extern int ExtractIconEx(string lpszFile, int niconIndex, IntPtr[] phiconLarge, IntPtr[] phiconSmall, int nIcons);

        [DllImport("User32.dll", CharSet = CharSet.Unicode)]
        internal static extern uint PrivateExtractIcons(string szFileName, int nIconIndex, int cxIcon, int cyIcon, IntPtr[] phicon, uint[] piconid, uint nIcons, uint flags);

        [DllImport("User32.dll")]
        internal static extern bool DestroyIcon(IntPtr hIcon);
    }

    public static class ImageUtil
    {
        public static ImageSource? ToImageSource(Bitmap bitmap)
        {
            MemoryStream stream = new();
            bitmap.Save(stream, ImageFormat.Png);

            return (ImageSource?)new ImageSourceConverter().ConvertFrom(stream);
        }

        public static System.Windows.Controls.Image ToImage(Bitmap bitmap)
        {
            return new System.Windows.Controls.Image()
            {
                Source = ToImageSource(bitmap)
            };
        }

        public static Bitmap? GetEXEIcon(string path)
        {
            IntPtr[] largeIcons, smallIcons;  //存放大/小图标的指针数组  
#pragma warning disable CS8625 // 无法将 null 字面量转换为非 null 的引用类型。
            int count = WinAPI.ExtractIconEx(path, -1, null, null, 999);
#pragma warning restore CS8625 // 无法将 null 字面量转换为非 null 的引用类型。
            if (count > 0)
            {
                largeIcons = new IntPtr[count];
                smallIcons = new IntPtr[count];

                WinAPI.ExtractIconEx(path, 0, largeIcons, smallIcons, count);
                if (largeIcons.Length > 0)
                {
                    Icon? icon = Icon.FromHandle(largeIcons[0]);
                    return icon.ToBitmap();
                }
                else if (smallIcons.Length > 0)
                {
                    Icon? icon = Icon.FromHandle(smallIcons[0]);
                    return icon.ToBitmap();
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }
    }
}

