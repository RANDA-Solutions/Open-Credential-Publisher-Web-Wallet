using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenCredentialPublisher.Data.Events
{
    public class VerityEvent
    {
        public VerityEvent() { }
        public VerityEvent(string id, string subject, object data, string eventType, DateTime eventTime, string topic = null)
        {
            Id = id;
            Topic = topic;
            Subject = subject;
            EventType = eventType;
            EventTime = eventTime;
            Data = data;
        }

        public string Id { get; set; }

        public string Topic { get; set; }

        public string Subject { get; set; }

        public string EventType { get; set; }

        public DateTime EventTime { get; set; }
        public object Data { get; set; }
    }
}
