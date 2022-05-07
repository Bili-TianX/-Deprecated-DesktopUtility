using Newtonsoft.Json.Linq;
using System.Security.Cryptography;
using System.Text;

public class AES
{
    public static void Main()
    {
        using StreamWriter writer = new(@"C:\Users\Bili_TianX\Desktop\5-15.love");
        using StreamReader reader = new(@"C:\Users\Bili_TianX\Desktop\1.json");
        writer.Write(AES.getInstsance().Encrypt(JArray.Parse(reader.ReadToEnd()).ToString()));
    }

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