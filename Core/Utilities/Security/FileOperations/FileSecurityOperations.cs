using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Core.Utilities.Security.FileOperations
{
    public class FileSecurityOperations
    {
        private static string KeyFilePath = Path.GetFullPath("../../../../../key.txt");
        private static string EncryptedFilePath = Path.GetFullPath("../../../../../connection.txt");
        public static void DecryptDBConnectionFile()
        {
            string tempFileName = Path.GetTempFileName();

            using (SymmetricAlgorithm cipher = Aes.Create())
            using (FileStream fileStream = File.OpenRead(EncryptedFilePath))
            using (FileStream tempFile = File.Create(tempFileName))
            {
                cipher.Key = Encoding.UTF8.GetBytes(GetKey());
                byte[] iv = new byte[cipher.BlockSize / 8];
                byte[] headerBytes = new byte[6];
                int remain = headerBytes.Length;

                while (remain != 0)
                {
                    int read = fileStream.Read(headerBytes, headerBytes.Length - remain, remain);

                    if (read == 0)
                    {
                        throw new EndOfStreamException();
                    }

                    remain -= read;
                }

                if (headerBytes[0] != 69 ||
                    headerBytes[1] != 74 ||
                    headerBytes[2] != 66 ||
                    headerBytes[3] != 65 ||
                    headerBytes[4] != 69 ||
                    headerBytes[5] != 83)
                {
                    throw new InvalidOperationException();
                }

                remain = iv.Length;

                while (remain != 0)
                {
                    int read = fileStream.Read(iv, iv.Length - remain, remain);

                    if (read == 0)
                    {
                        throw new EndOfStreamException();
                    }

                    remain -= read;
                }

                cipher.IV = iv;

                using (var cryptoStream =
                    new CryptoStream(tempFile, cipher.CreateDecryptor(), CryptoStreamMode.Write))
                {
                    fileStream.CopyTo(cryptoStream);
                }
            }

            File.Delete(EncryptedFilePath);
            File.Move(tempFileName, EncryptedFilePath);
        }

        public static void EncryptDBConnectionFile()
        {
            string tempFileName = Path.GetTempFileName();

            using (SymmetricAlgorithm cipher = Aes.Create())
            using (FileStream fileStream = File.OpenRead(EncryptedFilePath))
            using (FileStream tempFile = File.Create(tempFileName))
            {
                cipher.Key = Encoding.UTF8.GetBytes(GetKey());
                // aes.IV will be automatically populated with a secure random value
                byte[] iv = cipher.IV;

                // Write a marker header so we can identify how to read this file in the future
                tempFile.WriteByte(69);
                tempFile.WriteByte(74);
                tempFile.WriteByte(66);
                tempFile.WriteByte(65);
                tempFile.WriteByte(69);
                tempFile.WriteByte(83);

                tempFile.Write(iv, 0, iv.Length);

                using (var cryptoStream =
                    new CryptoStream(tempFile, cipher.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    fileStream.CopyTo(cryptoStream);
                }
            }

            File.Delete(EncryptedFilePath);
            File.Move(tempFileName, EncryptedFilePath);
        }

        private static string GenerateNewKey()
        {
            AesCryptoServiceProvider crypto = new AesCryptoServiceProvider();
            crypto.KeySize = 128;
            crypto.BlockSize = 128;
            crypto.GenerateKey();
            byte[] keyGenerated = crypto.Key;
            string key = Convert.ToBase64String(keyGenerated);
            File.WriteAllText(KeyFilePath, key);
            return key;
        }

        private static string GetKey()
        {
            string key = File.ReadAllText(KeyFilePath);

            if (string.IsNullOrEmpty(key))
                return GenerateNewKey();

            return key;
        }
    }
}
