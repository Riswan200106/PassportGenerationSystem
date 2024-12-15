using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;

namespace PassportGenerationSystem.Helper
{
    public static class Base64ConvertHelper
    {
        /// <summary>
        /// Converts a byte array to a Base64 string.
        /// </summary>
        /// <param name="fileBytes">Byte array representing the file.</param>
        /// <returns>Base64 encoded string.</returns>
        public static string ConvertToBase64(byte[] fileBytes)
        {
            if (fileBytes == null || fileBytes.Length == 0)
            {
                throw new ArgumentException("File bytes cannot be null or empty.");
            }

            return Convert.ToBase64String(fileBytes);
        }

        /// <summary>
        /// Converts a Base64 string back to a byte array.
        /// </summary>
        /// <param name="base64String">Base64 encoded string.</param>
        /// <returns>Byte array of the decoded file.</returns>
        public static byte[] ConvertFromBase64(string base64String)
        {
            if (string.IsNullOrWhiteSpace(base64String))
            {
                throw new ArgumentException("Base64 string cannot be null or empty.");
            }

            return Convert.FromBase64String(base64String);
        }

        /// <summary>
        /// Converts a file to a Base64 string.
        /// </summary>
        /// <param name="filePath">Path to the file.</param>
        /// <returns>Base64 encoded string of the file.</returns>
        public static string ConvertFileToBase64(string filePath)
        {
            if (string.IsNullOrEmpty(filePath) || !File.Exists(filePath))
            {
                throw new FileNotFoundException("File not found at the specified path.");
            }

            byte[] fileBytes = File.ReadAllBytes(filePath);
            return ConvertToBase64(fileBytes);
        }
    }
}
