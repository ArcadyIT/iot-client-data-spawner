using Microsoft.Azure.Devices;
using Microsoft.Azure.Devices.Client;
using Microsoft.Azure.Devices.Common.Exceptions;
using Newtonsoft.Json;
using System;
using System.Configuration;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IoT.Spawner.Application
{
    public class AzureIoTHub
    {
        /// <summary>
        /// 
        /// </summary>
        public const int Delay = 12000;

        /// <summary>
        /// Please replace with correct connection string value
        /// The connection string could be got from Azure IoT Hub -> Shared access policies -> iothubowner -> Connection String:
        /// </summary>
        private readonly string ConnectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

        /// <summary>
        /// Please replace with correct device connection string
        /// The device connect string could be got from Azure IoT Hub -> Devices -> {your device name } -> Connection string
        /// </summary>
        private readonly string DeviceConnectionString = ConfigurationManager.ConnectionStrings["DeviceConnectionString"].ConnectionString;

        /// <summary>
        /// 
        /// </summary>
        private readonly string DeviceId = ConfigurationManager.AppSettings["DeviceId"];

        /// <summary>
        /// 
        /// </summary>
        private readonly string IotHubName = ConfigurationManager.AppSettings["IotHubName"];

        /// <summary>
        /// 
        /// </summary>
        private const string IotHubD2CEndpoint = "messages/events";

        /// <summary>
        /// 
        /// </summary>
        private RegistryManager _registryManager;

        /// <summary>
        /// 
        /// </summary>
        private DeviceClient _deviceClient;

        /// <summary>
        /// 
        /// </summary>
        private Device device;

        public AzureIoTHub()
        {
            _registryManager = RegistryManager.CreateFromConnectionString(ConnectionString);
        }

        public async Task AddDeviceAsync()
        {
            try
            {
                device = await _registryManager.AddDeviceAsync(new Device(DeviceId));
            }
            catch (DeviceAlreadyExistsException)
            {
                device = await _registryManager.GetDeviceAsync(DeviceId);
            }
            Console.WriteLine("Generated device key: {0}", device.Authentication.SymmetricKey.PrimaryKey);

            var deviceAuthentication = new DeviceAuthenticationWithRegistrySymmetricKey(device.Id,
                device.Authentication.SymmetricKey.PrimaryKey);

            _deviceClient = DeviceClient.Create(IotHubName,
                deviceAuthentication,
                Microsoft.Azure.Devices.Client.TransportType.Mqtt);
        }

        public async Task SendDeviceToCloudMessageAsync(CancellationTokenSource cancelToken)
        {
            while (true)
            {
                if (cancelToken.IsCancellationRequested)
                    break;

                // Generate a random set of sensor readings
                var sensorReading = DataSpawner.GenerateRandomSensorReadingMessage();

                // Convert to a json message
                var messageString = JsonConvert.SerializeObject(sensorReading);
                var message = new Microsoft.Azure.Devices.Client.Message(Encoding.ASCII.GetBytes(messageString));
                await _deviceClient.SendEventAsync(message);

                Console.WriteLine("{0} > Sending message: {1}", DateTime.Now, messageString);

                await Task.Delay(Delay);
            }
        }
    }
}