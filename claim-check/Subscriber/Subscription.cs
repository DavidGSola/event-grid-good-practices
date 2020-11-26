// Default URL for triggering event grid function in the local environment.
// http://localhost:7071/runtime/webhooks/EventGrid?functionName={functionname}
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.EventGrid.Models;
using Microsoft.Azure.WebJobs.Extensions.EventGrid;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System.Text.Json;

namespace Subscriber
{
    public class Subscription
    {
        private readonly EventStorage _eventStorage;
        
        public Subscription()
        {
            _eventStorage = new BlobEventStorage();
        }

        [FunctionName("subscription")]
        public async Task Run([EventGridTrigger] EventGridEvent eventGridEvent, ILogger log)
        {
            EventGridEvent fullEvent = await _eventStorage.DownloadEvent(eventGridEvent);

            log.LogInformation(JsonSerializer.Serialize(fullEvent));
        }
    }}
