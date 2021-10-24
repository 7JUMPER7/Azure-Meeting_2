using System;
using System.Threading.Tasks;
using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;

namespace AzureQueuesExample
{
    class Program
    {
        static async Task Main(string[] args)
        {
            string connStr = "CONSTR";
            // SendMessage(connStr);
            string queueName = "sampleinput";
            QueueClient client = new QueueClient(connStr, queueName);
            var res = await client.PeekMessageAsync();
            System.Console.WriteLine(res.Value.Body);
        }

        static async Task SendMessage(string connStr)
        {
            string queueName = "sampleinput";
            QueueClient client = new QueueClient(connStr, queueName);
            await client.CreateIfNotExistsAsync();
            client.SendMessageAsync("Hello world", timeToLive: TimeSpan.FromSeconds(30));
            System.Console.WriteLine("Message sent");
        }
    }
}
