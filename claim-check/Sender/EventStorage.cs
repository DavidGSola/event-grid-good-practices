using Microsoft.Azure.EventGrid.Models;
using System.Threading.Tasks;

namespace Sender
{
    /// <summary>
    /// Represent a service to upload events
    /// </summary>
    public interface EventStorage
    {
        /// <summary>
        /// Upload an event payload to a external storage
        /// </summary>
        /// <param name="largeEvent">Event to upload</param>
        /// <returns>Event with a reference to its payload</returns>
        Task<EventGridEvent> UploadEventAsync(EventGridEvent largeEvent);
    }
}
