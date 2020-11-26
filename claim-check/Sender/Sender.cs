using Microsoft.Azure.EventGrid;
using Microsoft.Azure.EventGrid.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sender
{
    /// <summary>
    /// Represent a service to send events to an Event Grid Topic
    /// </summary>
    public class Sender
    {
        private static string TopicKey = "<-- Topic Key -->";
        private static string TopicEndpoint = "<-- Topic Endpoint -->";

        private readonly EventStorage _storage;

        public Sender(EventStorage storage)
        {
            _storage = storage;
        }

        /// <summary>
        /// Send a event to Event Grid Topic
        /// </summary>
        /// <returns>A task</returns>
        public async Task SendEventAsync()
        {
            TopicCredentials topicCredentials = new TopicCredentials(TopicKey);
            EventGridClient client = new EventGridClient(topicCredentials);

            EventGridEvent eventWithReference = await _storage.UploadEventAsync(GetEvent());

            await client.PublishEventsAsync(new Uri(TopicEndpoint).Host, new List<EventGridEvent>() { eventWithReference });
        }

        private static EventGridEvent GetEvent() =>
            new EventGridEvent()
            {
                Id = Guid.NewGuid().ToString(),
                EventType = "EventGrid.GoodPractices.ClaimCheck",
                Data = "mydata",
                EventTime = DateTime.Now,
                Subject = "LargeEvent",
                DataVersion = "1.0"
            };
    }
}
