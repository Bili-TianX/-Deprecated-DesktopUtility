using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DesktopUtility.Data
{
    internal class Love
    {
        private const string TargetFolder = "./data/love/";
        private const string TargetFile = "cache.json";
        public static List<string> list = new();
        public static List<string> used = new();

        public static string Get()
        {
            try
            {
                var unused = (from item in list
                             where !used.Contains(item)
                             select item).ToList();
                if (unused.Any())
                {
                    var tmp = unused[new Random().Next(0, unused.Count)];
                    used.Add(tmp);
                    return tmp;
                } else
                {
                    return "抱歉，暂时没有可用的句子";
                }
                
            } catch (Exception)
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
            JArray array = new();
            used.ForEach(x => array.Add(x));
            writer.Write(array.ToString());
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
                foreach (var x in JArray.Parse(reader.ReadToEnd()))
                {
                    used.Add(x.ToString());
                }
            }

            foreach (var file in Directory.EnumerateFiles(TargetFolder))
            {
                if (file.EndsWith(".love"))
                {
                    using StreamReader reader = new(file);
                    var s = AES.getInstsance().Decrypt(reader.ReadToEnd());
                    var array = JArray.Parse(s);
                    foreach (var a in array)
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
            if (instance == null) instance = new();
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
