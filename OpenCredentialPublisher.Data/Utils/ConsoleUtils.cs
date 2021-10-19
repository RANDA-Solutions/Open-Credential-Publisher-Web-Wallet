using Microsoft.Extensions.Logging;
using OpenCredentialPublisher.Data.Constants;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenCredentialPublisher.Data.Utils
{
    public static class ConsoleUtil
    {
        public static string Right(string value, int count)
        {
            return value.Substring(value.Length - count, count);
        }
        public static string Left(string value, int count)
        {
            return value.Substring(0, count);
        }
        public static void MoveFile(string currentLocation, string newLocation)
        {
            // To move a file or folder to a new location:
            System.IO.File.Move(currentLocation, newLocation);
        }
        public static void SaveFileBytes(string path, byte[] bytes)
        {
            File.WriteAllBytes(path, bytes);
        }
        public static void ConsoleSetColor(ConsoleColor consoleColor = Configuration.ConsoleColors.Default)
        {
            Console.ForegroundColor = consoleColor;
        }
        public static void ConsoleLine(string message, ConsoleColor consoleColor = Configuration.ConsoleColors.Default)
        {
            Console.ForegroundColor = consoleColor;
            Console.WriteLine(DateTime.UtcNow.ToString() + ' ' + message);
            Console.ForegroundColor = Configuration.ConsoleColors.Default;
        }
        public static void ConsoleLine(string message, ILogger log, ConsoleColor consoleColor = Configuration.ConsoleColors.Default)
        {
            ConsoleLine(message, consoleColor);
            log.LogInformation(message, null);
        }
        public static void ConsoleNewLine()
        {
            Console.Write($"\n");
        }
        public static void ConsoleWrite(string message, ConsoleColor consoleColor = Configuration.ConsoleColors.Default)
        {
            Console.ForegroundColor = consoleColor;
            Console.Write($"\r{DateTime.UtcNow.ToString()} {message}");
            Console.ForegroundColor = Configuration.ConsoleColors.Default;
        }
        
        public static string ToStringOrNull(object obj)
        {
            return obj?.ToString();
        }
        public static bool IsValidNonZeroInt(string value)
        {
            int val = -1;
            return int.TryParse(value, out val) && val > 0;
        }
        public static bool IsValidInt(string value)
        {
            int val = -1;
            return int.TryParse(value, out val) && val > -1;
        }
        
        public static string FormatNullableInt(int? num)
        {
            if (!num.HasValue)
                return "NULL";
            else
                return num.Value.ToString("N0", CultureInfo.InvariantCulture);
        }
        public static bool IsNotEmpty(object obj)
        {
            if (obj == null) return false;
            if (obj.ToString().Trim() != "") return true;
            return false;
        }
        
        public static Stream GenerateStreamFromString(string p)
        {
            Byte[] bytes = Encoding.ASCII.GetBytes(p);
            MemoryStream strm = new MemoryStream(bytes);
            return strm;
        }
    }
}
