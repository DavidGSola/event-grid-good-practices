using System;
using Microsoft.Azure.EventGrid.Models;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System.Text.Json;

namespace Subscriber
{
    /// <summary>
    /// Represent a service to download events from a Blob Storage
    /// </summary>
    public class BlobEventStorage : EventStorage
    {
        private static string ConnectionString = "<-- Connection String -->";

        /// <summary>
        /// Download asynchronously an event payload from a Blob
        /// </summary>
        /// <param name="eventWithReference">Event which include a reference to its payload</param>
        /// <returns>Event with both metadata and payload</returns>
        public async Task<EventGridEvent> DownloadEvent(EventGridEvent eventWithReference)
        {
            BlobReferenceData referenceData = JsonSerializer.Deserialize<BlobReferenceData>(eventWithReference.Data.ToString());

            CloudStorageAccount account = CloudStorageAccount.Parse(ConnectionString);
            CloudBlockBlob blob = new CloudBlockBlob(new Uri(referenceData.RefUri), account.Credentials);

            string rawData = await blob.DownloadTextAsync();

            return new EventGridEvent()
            {
                Id = eventWithReference.Id,
                EventType = eventWithReference.EventType,
                EventTime = eventWithReference.EventTime,
                Subject = eventWithReference.Subject,
                DataVersion = eventWithReference.DataVersion,
                Data = rawData
            };
        }
    }
}
