using System;
using System.IO;
using System.Security.Cryptography;

namespace VCrypt.Enc
{
    internal class AES
    {
        public AES() { }

        public string Encrypt(string rawstring, string password)
        {
            byte[] saltBytes = new byte[] { 98, 83, 33, 49, 82, 48, 107, 42, 116, 35, 52, 65, 56, 81, 125, 99, 42, 57, 115, 67, 51, 84, 94, 114, 69, 101, 54, 61, 105, 126, 77, 50, 89, 102, 53, 47, 38, 68, 110, 55, 78, 55, 123, 122, 53, 37, 113, 90, 41, 75, 48, 109, 119, 43, 80, 56, 74, 57, 40, 97, 64, 120, 54, 87, 52, 66, 100, 45, 36, 50, 88, 112, 51, 111, 63, 72, 70, 38, 49, 103, 121, 56, 76, 33, 42, 71, 106, 57, 116, 64, 49, 72, 63, 77, 111, 48, 119, 53, 81, 45, 40, 105, 52, 66, 115, 71, 123, 55, 37, 51, 87, 103, 67 };
            string outStr = null;
            RijndaelManaged aesAlg = null;
            try
            {
                Rfc2898DeriveBytes key = new Rfc2898DeriveBytes(password, saltBytes, 550000);
                aesAlg = new RijndaelManaged();
                aesAlg.Key = key.GetBytes(aesAlg.KeySize / 8);
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    msEncrypt.Write(BitConverter.GetBytes(aesAlg.IV.Length), 0, sizeof(int));
                    msEncrypt.Write(aesAlg.IV, 0, aesAlg.IV.Length);
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(rawstring);
                        }
                    }
                    outStr = Convert.ToBase64String(msEncrypt.ToArray());
                }
            }
            finally
            {
                if (aesAlg != null)
                    aesAlg.Clear();
            }
            return outStr;
        }

        public static byte[] ReadByteArray(Stream s)
        {
            byte[] rawLength = new byte[sizeof(int)];
            if (s.Read(rawLength, 0, rawLength.Length) != rawLength.Length)
            {
                throw new SystemException("Stream did not contain properly formatted byte array");
            }

            byte[] buffer = new byte[BitConverter.ToInt32(rawLength, 0)];
            if (s.Read(buffer, 0, buffer.Length) != buffer.Length)
            {
                throw new SystemException("Did not read byte array properly");
            }

            return buffer;
        }

        public string Decrypt(string encryptedString, string password)
        {
            byte[] saltBytes = new byte[] { 98, 83, 33, 49, 82, 48, 107, 42, 116, 35, 52, 65, 56, 81, 125, 99, 42, 57, 115, 67, 51, 84, 94, 114, 69, 101, 54, 61, 105, 126, 77, 50, 89, 102, 53, 47, 38, 68, 110, 55, 78, 55, 123, 122, 53, 37, 113, 90, 41, 75, 48, 109, 119, 43, 80, 56, 74, 57, 40, 97, 64, 120, 54, 87, 52, 66, 100, 45, 36, 50, 88, 112, 51, 111, 63, 72, 70, 38, 49, 103, 121, 56, 76, 33, 42, 71, 106, 57, 116, 64, 49, 72, 63, 77, 111, 48, 119, 53, 81, 45, 40, 105, 52, 66, 115, 71, 123, 55, 37, 51, 87, 103, 67 };
            RijndaelManaged aesAlg = null;
            string plaintext = null;
            try
            {
                Rfc2898DeriveBytes key = new Rfc2898DeriveBytes(password, saltBytes, 550000);
                byte[] bytes = Convert.FromBase64String(encryptedString);
                using (MemoryStream msDecrypt = new MemoryStream(bytes))
                {
                    aesAlg = new RijndaelManaged();
                    aesAlg.Key = key.GetBytes(aesAlg.KeySize / 8);
                    aesAlg.IV = ReadByteArray(msDecrypt);
                    ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }
            }
            finally
            {
                if (aesAlg != null)
                    aesAlg.Clear();
            }
            return plaintext;
        }
    }
}
