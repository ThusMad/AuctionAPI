﻿using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Services.Helpers.Utilities
{
    public static class Utility
    {
        public static byte[] GetHash(string inputString)
        {
            using HashAlgorithm algorithm = SHA256.Create();
                return algorithm.ComputeHash(Encoding.UTF8.GetBytes(inputString));
        }

        public static string GetHashString(string inputString)
        {
            var sb = new StringBuilder();
            foreach (var b in GetHash(inputString))
                sb.Append(b.ToString("X2"));

            return sb.ToString();
        }

        public static void RemoveImage(string dbPath)
        {
            var name = Path.GetFileName(dbPath);
            var localPath = Path.Combine("wwwroot", "images", "name");

            File.Delete(localPath);
        }
    }
}
