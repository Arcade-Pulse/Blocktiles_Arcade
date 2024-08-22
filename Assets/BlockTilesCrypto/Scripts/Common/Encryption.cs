using System;
using System.Text;

namespace CommonScripts
{
    public static class Encryption
    {
        private static string encryptionKey = "A5d!9fJ#2sL@8mQ^4rT&7nW*0xY@3vZ8"; // Ensure this is 16, 24, or 32 characters long

        public static string Encrypt(string plainText)
        {
            char[] key = encryptionKey.ToCharArray();
            char[] input = plainText.ToCharArray();
            char[] output = new char[input.Length];

            for (int i = 0; i < input.Length; i++)
            {
                output[i] = (char)(input[i] ^ key[i % key.Length]);
            }

            return Convert.ToBase64String(Encoding.UTF8.GetBytes(output));
        }

    }
}