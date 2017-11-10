using Microsoft.Azure.Devices.Client;
using Microsoft.Azure.Devices.Common.Exceptions;
using Microsoft.ServiceBus.Messaging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using IoT.Spawner.Application.Models;
using Devices = Microsoft.Azure.Devices;

namespace IoT.Spawner.Application
{
    public static class AzureIoTHub
    {
        public const int Delay = 12000;

        /// <summary>
        /// Please replace with correct connection string value
        /// The connection string could be got from Azure IoT Hub -> Shared access policies -> iothubowner -> Connection String:
        /// </summary>
        private static readonly string ConnectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

        /// <summary>
        /// Please replace with correct device connection string
        /// The device connect string could be got from Azure IoT Hub -> Devices -> {your device name } -> Connection string
        /// </summary>
        private static readonly string DeviceConnectionString = ConfigurationManager.ConnectionStrings["DeviceConnectionString"].ConnectionString;

        private const string IotHubD2CEndpoint = "messages/events";

        public static async Task<string> CreateDeviceIdentityAsync(string deviceName)
        {
            var registryManager = Devices.RegistryManager.CreateFromConnectionString(ConnectionString);
            Devices.Device device;
            try
            {
                device = await registryManager.AddDeviceAsync(new Devices.Device(deviceName));
            }
            catch (DeviceAlreadyExistsException)
            {
                device = await registryManager.GetDeviceAsync(deviceName);
            }
            return device.Authentication.SymmetricKey.PrimaryKey;
        }

        public static async Task SendDeviceToCloudMessageAsync(CancellationToken cancelToken)
        {
            var deviceClient = DeviceClient.CreateFromConnectionString(DeviceConnectionString);

            while (true)
            {
                if (cancelToken.IsCancellationRequested)
                    break;

                // Generate a random set of sensor readings
                var sensorReading = DataSpawner.GenerateRandomSensorReadingMessage();

                // Convert to a json message
                var messageString = JsonConvert.SerializeObject(sensorReading);
                var message = new Message(Encoding.ASCII.GetBytes(messageString));
                await deviceClient.SendEventAsync(message);
                Console.WriteLine("{0} > Sending message: {1}", DateTime.Now, messageString);

                await Task.Delay(Delay, cancelToken);
            }
        }

        public static async Task<string> ReceiveCloudToDeviceMessageAsync()
        {
            var deviceClient = DeviceClient.CreateFromConnectionString(DeviceConnectionString);

            while (true)
            {
                var receivedMessage = await deviceClient.ReceiveAsync();

                if (receivedMessage != null)
                {
                    var messageData = Encoding.ASCII.GetString(receivedMessage.GetBytes());
                    await deviceClient.CompleteAsync(receivedMessage);
                    return messageData;
                }

                await Task.Delay(TimeSpan.FromSeconds(1));
            }
        }

        public static async Task ReceiveMessagesFromDeviceAsync(CancellationToken cancelToken)
        {
            var eventHubClient = EventHubClient.CreateFromConnectionString(ConnectionString, IotHubD2CEndpoint);
            var d2CPartitions = eventHubClient.GetRuntimeInformation().PartitionIds;

            await Task.WhenAll(d2CPartitions.Select(partition => ReceiveMessagesFromDeviceAsync(eventHubClient, partition, cancelToken)));
        }

        private static async Task ReceiveMessagesFromDeviceAsync(EventHubClient eventHubClient, string partition, CancellationToken ct)
        {
            var eventHubReceiver = eventHubClient.GetDefaultConsumerGroup().CreateReceiver(partition, DateTime.UtcNow);
            while (true)
            {
                if (ct.IsCancellationRequested)
                    break;

                var eventData = await eventHubReceiver.ReceiveAsync(TimeSpan.FromSeconds(2));
                if (eventData == null) continue;

                var data = Encoding.UTF8.GetString(eventData.GetBytes());
                Console.WriteLine("Message received. Partition: {0} Data: '{1}'", partition, data);
            }
        }
    }
}
