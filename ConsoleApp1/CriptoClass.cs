using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace ConsoleApp1
{
    class CriptoClass 
    {
        private static RijndaelManaged set_cipher(string keyCrypt)
        {
            RijndaelManaged cipher = new RijndaelManaged();
            Rfc2898DeriveBytes pwdGen = new Rfc2898DeriveBytes(keyCrypt, Encoding.ASCII.GetBytes("чикчирик"), 10000);
            cipher.KeySize = 256;
            cipher.BlockSize = 256;
            cipher.Padding = PaddingMode.ISO10126;
            cipher.Mode = CipherMode.CBC;
            cipher.Key = pwdGen.GetBytes(cipher.KeySize / 8);
            return cipher;
        }

        public static String Encrypt(string str, string keyCrypt)
        {
            //RijndaelManaged cipher = new RijndaelManaged();
            //Rfc2898DeriveBytes pwdGen = new Rfc2898DeriveBytes(keyCrypt, Encoding.ASCII.GetBytes("чикчирик"), 10000);
            //cipher.KeySize = 256;
            //cipher.BlockSize = 256;
            //cipher.Padding = PaddingMode.ISO10126;
            //cipher.Mode = CipherMode.CBC;
            //cipher.Key = pwdGen.GetBytes(cipher.KeySize / 8);

            var cipher = set_cipher(keyCrypt);

            ICryptoTransform t = cipher.CreateEncryptor();
            byte[] textInBytes = Encoding.UTF8.GetBytes(str);
            byte[] result = t.TransformFinalBlock(textInBytes, 0, textInBytes.Length);
            return Convert.ToBase64String(result);
            //Console.WriteLine(Convert.ToBase64String(result));
            //return Encoding.UTF8.GetString(result);
            //return result;
        }
        public static string Decrypt(string result, string keyCrypt)
        {
            //RijndaelManaged cipher = new RijndaelManaged();
            //Rfc2898DeriveBytes pwdGen = new Rfc2898DeriveBytes(keyCrypt, Encoding.ASCII.GetBytes("чикчирик"), 10000);
            //cipher.KeySize = 256;
            //cipher.BlockSize = 256;
            //cipher.Padding = PaddingMode.ISO10126;
            //cipher.Mode = CipherMode.CBC;
            //cipher.Key = pwdGen.GetBytes(cipher.KeySize / 8);
            var cipher = set_cipher(keyCrypt);

            ICryptoTransform t2 = cipher.CreateDecryptor();

            byte[] textInBytes = Convert.FromBase64String(result);
            //byte[] textInBytes = Encoding.UTF8.GetBytes(result);
            byte[] result2 = t2.TransformFinalBlock(textInBytes, 0, textInBytes.Length);
            Console.WriteLine(Encoding.UTF8.GetString(result2));
            return Encoding.UTF8.GetString(result2);
        }
    }
}
