using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Linq;

namespace CloudEventSubscriber
{
    public static class CloudEventSubscription
    {
        // Azure Function for handling Event Grid events using CloudEventSchema v1.0 
        // (see CloudEvents Specification: https://github.com/cloudevents/spec)
        [FunctionName("CloudEventSubscription")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "options", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            // Handle EventGrid subscription validation for CloudEventSchema v1.0.
            // It sets the header response `Webhook-Allowed-Origin` with the value from 
            // the header request `Webhook-Request-Origin` 
            // (see: https://docs.microsoft.com/en-us/azure/event-grid/cloudevents-schema#use-with-azure-functions)
            if (HttpMethods.IsOptions(req.Method))
            {
                if (req.Headers.TryGetValue("Webhook-Request-Origin", out var headerValues))
                {
                    var originValue = headerValues.FirstOrDefault();
                    if (!string.IsNullOrEmpty(originValue))
                    {
                        req.HttpContext.Response.Headers.Add("Webhook-Allowed-Origin", originValue);
                        return new OkResult();
                    }

                    return new BadRequestObjectResult("Missing 'Webhook-Request-Origin' header when validating");
                }
            }

            // Handle an event received from EventGrid.
            if (HttpMethods.IsPost(req.Method))
            {
                string @event = await new StreamReader(req.Body).ReadToEndAsync();
                log.LogInformation("Event recevied: {event}", @event);
            }

            return new OkResult();
        }
    }
}
