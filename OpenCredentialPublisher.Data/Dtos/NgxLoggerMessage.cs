using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace OpenCredentialPublisher.Data.Dtos
{
    public class NgxLoggerMessage
    {
        [JsonPropertyName("level")]
        public int Level { get; set; }
        [JsonPropertyName("timestamp")]
        public string Timestamp { get; set; }
        [JsonPropertyName("fileName")]
        public string FileName { get; set; }
        [JsonPropertyName("lineNumber")]
        public string LineNumber { get; set; }
        [JsonPropertyName("message")]
        public string Message { get; set; }
        [JsonPropertyName("additional")]
        public string[] Additional { get; set; }
    }

    public class NgxLoggerLevel
    {
        public const string Trace = "TRACE";
        public const string Debug = "DEBUG";
        public const string Info = "INFO";
        public const string Log = "LOG";
        public const string Warn = "WARN";
        public const string Error = "ERROR";
        public const string Fatal = "FATAL";
        public const string Off = "OFF";
    }

    public enum NgxLoggerLevelEnum
    {
       Trace = 0,
       Debug = 1,
       Info = 2,
       Log = 3,
       Warn = 4,
       Error =5,
       Fatal = 6,
       Off = 7
    }
}
