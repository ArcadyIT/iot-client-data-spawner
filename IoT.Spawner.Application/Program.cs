using System;
using System.Threading;
using System.Threading.Tasks;

namespace IoT.Spawner.Application
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //Sample 1: Create device if you didn't have one in Azure IoT Hub, FIRST YOU NEED SPECIFY connectionString first in AzureIoTHub.cs
//            CreateDeviceIdentity();

            //Sample 2: comment above line and uncomment following line, FIRST YOU NEED SPECIFY connectingString and deviceConnectionString in AzureIoTHub.cs
            SimulateDeviceToSendD2CAndReceiveD2C();
        }

        public static void CreateDeviceIdentity()
        {
            var deviceName = "DataSpawner";
            AzureIoTHub.CreateDeviceIdentityAsync(deviceName).Wait();
            Console.WriteLine($"Device with name '{deviceName}' was created/retrieved successfully");
        }

        private static void SimulateDeviceToSendD2CAndReceiveD2C()
        {
            var tokenSource = new CancellationTokenSource();

            Console.CancelKeyPress += (s, e) =>
            {
                e.Cancel = true;
                tokenSource.Cancel();
                Console.WriteLine("Exiting...");
            };
            Console.WriteLine("Press CTRL+C to exit");

            try
            {
                Task.WaitAll(AzureIoTHub.SendDeviceToCloudMessageAsync(tokenSource.Token));   
            }
            catch
            {
                return;
            }
        }
    }
}
