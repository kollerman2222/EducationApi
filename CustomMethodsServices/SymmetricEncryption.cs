using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Security.Cryptography;
using System.Text;

namespace FgssrApi.CustomMethodsServices
{
    public static class SymmetricEncryption
    {
       

        public static string Encrypt(string textToEncrypt , string encryptionKey)
        {
            var textToBytes = Encoding.UTF8.GetBytes(textToEncrypt);
            var key = Encoding.UTF8.GetBytes(encryptionKey);

            using (Aes aes = Aes.Create())
            {
                aes.Key = key;
                aes.IV = new byte[16]; // generate random later to get different encryption if same text is applied again
                aes.Mode = CipherMode.CBC;

                ICryptoTransform encryptor = aes.CreateEncryptor();
                var encryptedText = encryptor.TransformFinalBlock(textToBytes, 0, textToBytes.Length);

                return Convert.ToBase64String(encryptedText);
            }
        }


        public static string Decrypt(string encryptedText , string encryptionKey)
        {
            var textToBytes = Convert.FromBase64String(encryptedText);
            var key = Encoding.UTF8.GetBytes(encryptionKey);

            using (Aes aes = Aes.Create())
            {
                aes.Key = key;
                aes.IV = new byte[16]; // generate random later to get different encryption if same text is applied again
                aes.Mode= CipherMode.CBC;

                ICryptoTransform decryptor = aes.CreateDecryptor();
                var decryptedText = decryptor.TransformFinalBlock(textToBytes, 0, textToBytes.Length);

                return Encoding.UTF8.GetString(decryptedText);
            }
        }

    }
}
