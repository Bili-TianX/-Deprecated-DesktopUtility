using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace DesktopUtility.Data
{
    internal class Love
    {
        private const string TargetFolder = "./data/love/";
        private const string TargetFile = "cache.json";
        public static List<string> list = new();
        public static Dictionary<(int year, int month, int day), string> used = new();

        public static string Get()
        {
            try
            {
                DateTime tmp = DateTime.Now;
                (int year, int month, int day) now = (tmp.Year, tmp.Month, tmp.Day);
                if (!used.ContainsKey(now))
                {
                    List<string>? unused = (from item in list
                                            where !used.ContainsValue(item)
                                            select item).ToList();

                    if (unused.Any())
                    {
                        string? s = unused[Random.Shared.Next(0, unused.Count)];
                        used[now] = s;
                        return s;
                    }
                    else
                    {
                        return "抱歉，暂时没有可用的句子";
                    }

                }
                else
                {
                    return used[now];
                }

            }
            catch (Exception)
            {
                return "抱歉，暂时没有可用的句子";
            }
        }

        public static void SaveToFile()
        {
            if (!Directory.Exists(TargetFolder))
            {
                Directory.CreateDirectory(TargetFolder);
            }

            using StreamWriter writer = new(TargetFolder + TargetFile);
            JObject obj = new();
            foreach (KeyValuePair<(int year, int month, int day), string> item in used)
            {
                (int year, int month, int day) key = item.Key;
                obj.Add($"{key.year}-{key.month}-{key.day}", item.Value);
            }
            writer.Write(obj.ToString());
        }

        public static void LoadFromFile()
        {
            if (!Directory.Exists(TargetFolder))
            {
                Directory.CreateDirectory(TargetFolder);
                return;
            }
            if (File.Exists(TargetFolder + TargetFile))
            {
                using StreamReader reader = new(TargetFolder + TargetFile);
                foreach (KeyValuePair<string, JToken?> x in JObject.Parse(reader.ReadToEnd()))
                {
                    string[]? key = x.Key.Split('-');
                    string? value = x.Value!.ToString();
                    used.Add((int.Parse(key[0]), int.Parse(key[1]), int.Parse(key[2])), value);
                }
            }

            foreach (string? file in Directory.EnumerateFiles(TargetFolder))
            {
                if (file.EndsWith(".love"))
                {
                    using StreamReader reader = new(file);
                    string? s = AES.getInstsance().Decrypt(reader.ReadToEnd());
                    JArray? array = JArray.Parse(s);
                    foreach (JToken? a in array)
                    {
                        list.Add(a.ToString());
                    }
                }
            }
        }
    }



    public class AES
    {
        private static AES? instance;
        private const string Key = "8e054ee5a4b91cc92fdb4d78ec66d96c";

#pragma warning disable SYSLIB0022 // 类型或成员已过时
        private readonly RijndaelManaged aes = new();
        private readonly ICryptoTransform Encryptor;
        private readonly ICryptoTransform Decryptor;
#pragma warning restore SYSLIB0022 // 类型或成员已过时

        public static AES getInstsance()
        {
            if (instance == null)
            {
                instance = new();
            }

            return instance;
        }

        private AES()
        {
            aes.Key = Encoding.UTF8.GetBytes(Key);
            aes.Mode = CipherMode.ECB;
            aes.Padding = PaddingMode.PKCS7;

            Encryptor = aes.CreateEncryptor();
            Decryptor = aes.CreateDecryptor();
        }

        public string Encrypt(string s)
        {
            byte[] source = Encoding.UTF8.GetBytes(s);

            byte[] resultArray = Encryptor.TransformFinalBlock(source, 0, source.Length);
            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }

        public string Decrypt(string s)
        {
            byte[] source = Convert.FromBase64String(s);

            byte[] result = Decryptor.TransformFinalBlock(source, 0, source.Length);
            return Encoding.UTF8.GetString(result);
        }
    }
}
