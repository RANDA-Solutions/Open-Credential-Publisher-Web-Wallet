using Azure.Messaging.EventGrid;
using Microsoft.Azure.Relay;
//using Newtonsoft.Json;
using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace OpenCredentialPublisher.AzureListener
{
    class Program
    {
        static void Main(string[] args)
        {
            var usingDocker = false;
            var port = usingDocker ? 30658 : 7071;
            var listener = new Listener("Endpoint=sb://open-credential-publisher.servicebus.windows.net/;SharedAccessKeyName=ListenOnly;SharedAccessKey=8eRa3OIA4cUYPY6ZHT15py0HmC1mMBWQSc0i5YGaD0g=", "verity-event-listener", (message) =>
            {
                try
                {
                    var client = new HttpClient();
                    if (message != null && message.Contains(@"\u"))
                    {
                        message = message.Replace(@"\u0022", "\"");
                    }

                    if (message != null && (message.StartsWith('"') || message.EndsWith('"')))
                    {
                        message = message.Trim('"');
                    }

                    client.PostAsync($"http://localhost:{port}/api/verity", new ByteArrayContent(Encoding.UTF8.GetBytes(message))).GetAwaiter().GetResult();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            });

            listener.listen();
            Console.ReadLine();
        }
    }

    public class Listener
    {
        public delegate void OnListen(string message);

        private string ConnectionString { get; }
        private string ConnectionName { get; }
        OnListen handler;
        private HybridConnectionListener listener = null;

        public Listener(string connectionString, string connectionName, OnListen handler)
        {
            ConnectionString = connectionString;
            ConnectionName = connectionName;
            this.handler = handler;
        }

        public void listen()
        {
            listener = new HybridConnectionListener(ConnectionString, ConnectionName);

            listener.RequestHandler = (context) =>
            {
                ProcessEventGridEvents(context);
                context.Response.StatusCode = System.Net.HttpStatusCode.OK;
                context.Response.Close();
            };

            listener.OpenAsync().GetAwaiter().GetResult();

        }

        private void ProcessEventGridEvents(RelayedHttpListenerContext context)
        {
            var content = new StreamReader(context.Request.InputStream).ReadToEnd();

            var eventGridEvents = EventGridEvent.ParseMany(BinaryData.FromString(content));

            var options = new JsonSerializerOptions
            {
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            };

            foreach (var eventGridEvent in eventGridEvents)
            {
                //Console.WriteLine($"Received event {eventGridEvent.Id} with type:{eventGridEvent.EventType} data: {JsonConvert.SerializeObject(eventGridEvent.Data)}");

                var eventGridData = JsonSerializer.Deserialize<dynamic>(eventGridEvent.Data.ToString());
                if (eventGridData is JsonElement)
                {
                    var element = (JsonElement)eventGridData;
                    if (element.ValueKind == JsonValueKind.String)
                    {
                        var elementString = element.GetString();
                        handler(elementString);
                    }
                }
                else
                {
                    File.WriteAllText(Path.Combine(Directory.GetCurrentDirectory(), $"{eventGridEvent.Id}.json"), JsonSerializer.Serialize(eventGridEvent.Data, options));
                    handler(eventGridEvent.Data.ToString());
                }

            }
        }

        public void stop()
        {
            if (!listener?.IsOnline ?? default)
                return;
            listener.CloseAsync().GetAwaiter().GetResult();
        }
    }
}
