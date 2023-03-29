using System.Security.Cryptography;
using System.Text;

namespace Security;

internal class Program
{
    static void Main(string[] args)
    {
        //TestHash();
        //TestSymmetric();
        //TestAsymmetric();
        //TestAsymmetricConfidentiality();
        TestSymmetricConfidentiality();
    }

    private static void TestSymmetricConfidentiality()
    {
        // Sender
        string text = "Hello World";
        Aes aes = Aes.Create();
        //aes.Mode = CipherMode.CBC;
        byte[] key = aes.Key;
        byte[] iv = aes.IV;


        byte[] data;
        using (MemoryStream mem = new MemoryStream())
        {
            using (CryptoStream crypto = new CryptoStream(mem, aes.CreateEncryptor(), CryptoStreamMode.Write))
            {
                using (StreamWriter sw = new StreamWriter(crypto))
                {
                    sw.WriteLine(text);
                }
            }
            data = mem.ToArray();
        }
        Console.WriteLine(Convert.ToBase64String(data));

        // Receiver
        Aes aes2= Aes.Create();
        //aes2.Mode = CipherMode.CBC;
        aes2.Key = key;
        aes2.IV = iv;   

        using(MemoryStream mem = new MemoryStream(data)) 
        { 
            using(CryptoStream crypto = new CryptoStream(mem, aes2.CreateDecryptor(), CryptoStreamMode.Read))
            {
                using (StreamReader rd = new StreamReader(crypto))
                {
                    string txt = rd.ReadToEnd();
                    Console.WriteLine(txt);
                }
            }
        }

    }

    private static void TestAsymmetricConfidentiality()
    {
        // Done by the receiver
        var rsa = RSA.Create();
        string pubKey = rsa.ToXmlString(false);
        string privKey = rsa.ToXmlString(true);

        // Sender
        string text = "Hello World";
        rsa = RSA.Create();
        rsa.FromXmlString(pubKey);
        byte[] cipher = rsa.Encrypt(Encoding.UTF8.GetBytes(text), RSAEncryptionPadding.Pkcs1);
        Console.WriteLine(Convert.ToBase64String(cipher));


        // Receiver
        rsa = RSA.Create();
        rsa.FromXmlString(privKey);
        byte[] original = rsa.Decrypt(cipher, RSAEncryptionPadding.Pkcs1);
        Console.WriteLine(Encoding.UTF8.GetString(original));

    }

    private static void TestAsymmetric()
    {
        string text = "Hello World";
        var algorithm = SHA1.Create();
        byte[] hash = algorithm.ComputeHash(Encoding.UTF8.GetBytes(text));
        DSA dsa = DSA.Create();
        string pubKey = dsa.ToXmlString(false);
        string privKey = dsa.ToXmlString(true);
        Console.WriteLine(privKey);
        byte[] signature = dsa.SignData(hash, HashAlgorithmName.SHA1);


        // Receiver
        hash = algorithm.ComputeHash(Encoding.UTF8.GetBytes(text));
        dsa = DSA.Create();
        dsa.FromXmlString(pubKey);
        bool notTampered = dsa.VerifyData(hash, signature, HashAlgorithmName.SHA1);
        Console.WriteLine(notTampered);
    }

    private static void TestSymmetric()
    {
        // Sender
        string text = "Hello World";
        var algorithm = new HMACSHA1();
        byte[] key = algorithm.Key;

        byte[] hash = algorithm.ComputeHash(Encoding.UTF8.GetBytes(text));
        Console.WriteLine(Convert.ToBase64String(hash));

        // Receiver
        algorithm = new HMACSHA1();
        algorithm.Key = key;
        hash = algorithm.ComputeHash(Encoding.UTF8.GetBytes(text));
        Console.WriteLine(Convert.ToBase64String(hash));
        hash = algorithm.ComputeHash(Encoding.UTF8.GetBytes(text + "."));
        Console.WriteLine(Convert.ToBase64String(hash));

    }

    private static void TestHash()
    {
        string text = "Hello World";
        var algorithm = SHA1.Create();
        byte[] hash = algorithm.ComputeHash(Encoding.UTF8.GetBytes(text));
        Console.WriteLine(Convert.ToBase64String(hash));
        hash = algorithm.ComputeHash(Encoding.UTF8.GetBytes(text));
        Console.WriteLine(Convert.ToBase64String(hash));
        hash = algorithm.ComputeHash(Encoding.UTF8.GetBytes(text+"."));
        Console.WriteLine(Convert.ToBase64String(hash));
    }
}