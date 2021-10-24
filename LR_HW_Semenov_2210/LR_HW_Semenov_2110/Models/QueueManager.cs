using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClassLib;
using Azure.Storage.Queues;
using System.Text.Json;
using System.Text.Json.Serialization;
using Azure.Storage.Queues.Models;

namespace LR_HW_Semenov_2110.Models
{
    public class QueueManager
    {
        private readonly QueueClient client;
        public QueueManager(QueueClient client)
        {
            this.client = client;
            client.CreateIfNotExists();
        }

        public async Task AddLot(Lot lot)
        {
            string data = JsonSerializer.Serialize(lot);
            await client.SendMessageAsync(data, timeToLive: TimeSpan.FromDays(1));
        }

        public async Task<Lot> GetLot()
        {
            QueueMessage message = await client.ReceiveMessageAsync(visibilityTimeout: TimeSpan.FromSeconds(1));
            if (message != null)
            {
                Lot lot = JsonSerializer.Deserialize<Lot>(message.Body);
                lot.MessageId = message.MessageId;
                lot.PopReceipt = message.PopReceipt;
                return lot;
            }
            return null;
        }

        public async Task<bool> RemoveLot(string messageId, string popReceipt)
        {
            try
            {
                await client.DeleteMessageAsync(messageId, popReceipt);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
