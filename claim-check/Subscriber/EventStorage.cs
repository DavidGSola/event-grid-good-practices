using Microsoft.Azure.EventGrid.Models;
using System.Threading.Tasks;

namespace Subscriber
{
    /// <summary>
    /// Represent a service to download events
    /// </summary>
    public interface EventStorage
    {
        /// <summary>
        /// Download asynchronously an event payload
        /// </summary>
        /// <param name="eventWithReference">Event which include a reference to its payload</param>
        /// <returns>Event with both metadata and payload</returns>
        Task<EventGridEvent> DownloadEvent(EventGridEvent eventWithReference);
    }
}
