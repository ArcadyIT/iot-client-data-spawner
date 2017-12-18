using System;
using System.Threading;
using System.Threading.Tasks;

namespace IoT.Spawner.Application
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Simulated device\n");

            CancellationTokenSource cts = new CancellationTokenSource();

            Console.CancelKeyPress += (s, e) =>
            {
                e.Cancel = true;
                cts.Cancel();
                Console.WriteLine("Exiting...");
            };

            var hub = new AzureIoTHub();
            Task.WaitAll(hub.AddDeviceAsync());

            Task.WaitAll(hub.SendDeviceToCloudMessageAsync(cts));
        }
    }
}