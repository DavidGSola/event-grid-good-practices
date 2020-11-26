using Microsoft.Azure.EventGrid.Models;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System.Text.Json;
using System.Threading.Tasks;

namespace Sender
{
    /// Represent a service to upload events to a Blob Storage
    /// </summary>
    public class BlobEventStorage : EventStorage
    {
        private static string ConnectionString = "<-- Connection String -->";

        /// <summary>
        /// Upload an event payload to a Blob Storage
        /// </summary>
        /// <param name="largeEvent">Event to upload</param>
        /// <returns>Event with a reference to its payload</returns>
        public async Task<EventGridEvent> UploadEventAsync(EventGridEvent largeEvent)
        {
            CloudStorageAccount account = CloudStorageAccount.Parse(ConnectionString);
            CloudBlobClient client = account.CreateCloudBlobClient();
            CloudBlobContainer container = client.GetContainerReference("event-storage");

            CloudBlockBlob blob = container.GetBlockBlobReference(largeEvent.Id);
                
            await blob.UploadTextAsync(JsonSerializer.Serialize(largeEvent.Data));

            return new EventGridEvent()
            {
                Id = largeEvent.Id,
                EventType = largeEvent.EventType,
                EventTime = largeEvent.EventTime,
                Subject = largeEvent.Subject,
                DataVersion = largeEvent.DataVersion,
                Data = new BlobReferenceData()
                {
                    RefUri = blob.Uri.ToString()
                }
            };
        }
    }
}
