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
        internal static System.Windows.Media.Color Black = System.Windows.Media.Color.FromRgb(0, 0, 0);
    }

    public static class WinAPI
    {
        [DllImport("User32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        internal static extern ulong GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("User32.dll")]
        internal static extern int SetWindowLong(IntPtr hWnd, int nIndex, ulong dwNewLong);

        [DllImport("user32.dll")]
        internal static extern IntPtr GetDesktopWindow();

        [DllImport("User32.dll")]
        internal static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

        [DllImport("User32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        internal static extern IntPtr FindWindow(string lpWindowClass, string lpWindowName);

        [DllImport("User32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        internal static extern IntPtr FindWindowEx(IntPtr parentHandle, IntPtr childAfter, string className, string windowTitle);

        [DllImport("shell32.dll", CharSet = CharSet.Unicode)]
        internal static extern unsafe int ExtractIconEx(string lpszFile, int niconIndex, IntPtr[] phiconLarge, IntPtr[] phiconSmall, int nIcons);

        [DllImport("User32.dll", CharSet = CharSet.Unicode)]
        internal static extern unsafe bool DestroyIcon(IntPtr hIcon);

        [DllImport("User32.dll", CharSet = CharSet.Unicode)]
        internal static extern unsafe bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

        [DllImport("user32.dll")]
        internal static extern int SetParent(int hWndChild, int hWndNewParent);
    }

    public static class ImageUtil
    {
        public static ImageSource? ToImageSource(Bitmap bitmap)
        {
            MemoryStream stream = new();
            bitmap.Save(stream, ImageFormat.Png);

            return (ImageSource?)new ImageSourceConverter().ConvertFrom(stream);
        }

        public static MemoryStream ToStream(Bitmap bitmap)
        {
            MemoryStream stream = new();
            bitmap.Save(stream, ImageFormat.Icon);
            return stream;
        }

        public static System.Windows.Controls.Image ToImage(Bitmap bitmap)
        {
            return new System.Windows.Controls.Image()
            {
                Source = ToImageSource(bitmap)
            };
        }



        public static unsafe Bitmap GetEXEIcon(string path)
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
            }

            return DesktopUtility.Resources.Resource1.app_default_icon;
        }
    }
}

