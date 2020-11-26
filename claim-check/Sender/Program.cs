using System.Threading.Tasks;

namespace Sender
{
    partial class Program
    {
        static async Task Main()
        {
            Sender sender = new Sender(new BlobEventStorage());
            await sender.SendEventAsync();
        }
    }
}
